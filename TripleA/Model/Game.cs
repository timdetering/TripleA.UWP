using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Xml.Linq;
using System.Diagnostics;
using TripleA.Attachments;
using System.Collections.ObjectModel;
using System.IO;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using TripleA.Events;

namespace TripleA.Model
{
    public class Game : ViewModelBase
    {
        private string scenario = null;
        public string Scenario
        {
            get { return scenario; }
        }

        private static Game instance;
        public static Game Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new Game();
                }
                return instance;
            }
        }

        public Map Map { get; set; }
        internal List<Player> Players { get; private set; }
        public List<Alliance> Alliances { get; private set; }
        public List<UnitType> UnitTypes { get; private set; }
        public List<ResourceType> ResourceTypes { get; set; }
        public List<SequenceStepType> SequenceDelegates { get; set; }
        public List<SequenceStep> Sequence { get; set; }
        public List<ProductionRule> ProductionRules { get; set; }
        internal List<ProductionFrontier> ProductionFrontiers { get; private set; }
        public List<GameSetting> Settings { get; private set; }
        public bool HasCustomUnitGraphics { get; private set; }

        private Game()
        {
            this.Players = new List<Player>();
        }

        public async Task Initialize(string scenario, string configFile)
        {
            this.scenario = scenario;

            await LoadMap();
            await LoadGameConfig(configFile);

            Messenger.Default.Send<GameInitializationCompleted>(new GameInitializationCompleted());
        }


        private async Task LoadMap()
        {
            this.Map = new Map();
            var configFilePath = "ms-appx:///Game/" + scenario + "/map.properties";
            Uri dataUri = new Uri(configFilePath);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var configFileText = await FileIO.ReadTextAsync(file);

            var allProperties = configFileText.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Debug.WriteLine("Property Line Count: " + allProperties.Length);

            foreach(var prop in allProperties)
            {
                var propSplit = prop.Split(new char[] { '=' });
                if(propSplit.Length == 2)
                {
                    if(!this.Map.Properties.ContainsKey(propSplit[0]))
                    {
                        this.Map.Properties.Add(propSplit[0], propSplit[1]);
                    }
                }
            }

            Debug.WriteLine("Property Count: " + this.Map.Properties.Count);

            var hasRelief = false;
            if(this.Map.Properties.ContainsKey("map.hasRelief"))
            {
                bool.TryParse(this.Map.Properties["map.hasRelief"], out hasRelief);
                this.Map.HasRelief = hasRelief;
            }

            var width = 0;
            if (this.Map.Properties.ContainsKey("map.width"))
            {
                int.TryParse(this.Map.Properties["map.width"], out width);
                this.Map.Width = width;
                this.Map.ColumnCount = width / 256;
                this.Map.TileWidth = width + 256;
            }
            
            var height = 0;
            if (this.Map.Properties.ContainsKey("map.height"))
            {
                int.TryParse(this.Map.Properties["map.height"], out height);
                this.Map.Height = height;
                this.Map.RowCount = height / 256;
                this.Map.TileHeight = height + 256;
            }

            Debug.WriteLine("C x R: " + this.Map.ColumnCount + " x " + this.Map.RowCount);
            Debug.WriteLine("X x Y: " + this.Map.Width + " x " + this.Map.Height);

            for(int c = 0; c <= this.Map.ColumnCount; c++)
            {
                for(int r = 0; r <= this.Map.RowCount; r++)
                {
                    this.Map.BaseTiles.Add(new Tile() { ImagePath = "ms-appx:///Game/" + scenario + "/baseTiles/" + c + "_" + r + ".png" });
                    this.Map.ReliefTiles.Add(new Tile() { ImagePath = "ms-appx:///Game/" + scenario + "/reliefTiles/" + c + "_" + r + ".png" });
                }
            }
            Debug.WriteLine("Base Tiles: " + this.Map.BaseTiles.Count);
            Debug.WriteLine("Relief Tiles: " + this.Map.ReliefTiles.Count);
        }

        private async Task LoadCenters()
        {
            var configFilePath = "ms-appx:///Game/" + scenario + "/centers.txt";
            Uri dataUri = new Uri(configFilePath);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var centersTextFile = await FileIO.ReadTextAsync(file);

            var allTerritoryCenterPoints = centersTextFile.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach(var centerPointLine in allTerritoryCenterPoints)
            {
                var lineSplit = centerPointLine.Split(new char[] { '(' }, StringSplitOptions.RemoveEmptyEntries);
                var territoryName = lineSplit[0].Trim();
                var coordinate = lineSplit[1].Replace(")", "");
                var coordinateSplit = coordinate.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                var x = int.Parse(coordinateSplit[0].Trim());
                var y = int.Parse(coordinateSplit[1].Trim());
                var point = new Point(x, y);

                var territory = this.Map.Territories.Where(f => f.Name == territoryName).FirstOrDefault();

                if(territory != null)
                {
                    territory.CenterPoint = point;
                }
                else
                {
                    Debug.WriteLine("Unable to find territory: " + territoryName);
                }
            }
        }

        private async Task LoadCapitols()
        {
            var configFilePath = "ms-appx:///Game/" + scenario + "/capitols.txt";
            Uri dataUri = new Uri(configFilePath);

            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
                var polygonTextFile = await FileIO.ReadTextAsync(file);

                var capitols = polygonTextFile.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                //this.Map.Capitols = new ObservableCollection<Capitol>();

                foreach (var capitolLine in capitols)
                {
                    var lineSplit = capitolLine.Split(new char[] { '(' }, StringSplitOptions.RemoveEmptyEntries);
                    var territoryName = lineSplit[0].Trim();
                    var coordinate = lineSplit[1].Replace(")", "");
                    var coordinateSplit = coordinate.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                    var x = int.Parse(coordinateSplit[0].Trim());
                    var y = int.Parse(coordinateSplit[1].Trim());
                    var point = new Point(x, y);

                    var territory = this.Map.Territories.Where(f => f.Name == territoryName).FirstOrDefault();
                    territory.IsCapitol = true;

                    var newCap = new Capitol();
                    newCap.Territory = territory;
                    newCap.Point = point;
                    newCap.ImagePath = "ms-appx:///Game/assets/flags/" + territory.OriginalOwner.Name + "_large.png";

                    this.Map.Capitols.Add(newCap);
                }
            }
            catch (FileNotFoundException fnfe)
            {
                Debug.WriteLine(dataUri.ToString() + " does not exist!");
            }
        }

        private async Task LoadPlacementLocations()
        {
            var configFilePath = "ms-appx:///Game/" + scenario + "/place.txt";
            Uri dataUri = new Uri(configFilePath);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var placementTextFile = await FileIO.ReadTextAsync(file);

            var allTerritoryPlacements = placementTextFile.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Debug.WriteLine("Place Count: " + allTerritoryPlacements.Length);

            foreach(var placement in allTerritoryPlacements)
            {
                var firstPointIndex = placement.IndexOf('(');
                var territoryName = placement.Substring(0, firstPointIndex).Trim();
                
                var matchingTerritory = this.Map.Territories.Where(f => f.Name == territoryName).FirstOrDefault();

                if (matchingTerritory != null)
                {
                    var justPoints = placement.Replace(territoryName, "").Trim();
                    var pointList = new List<Point>();
                    //(1884,1292) (1961,1292) (1962,1293) (1962,1485) (2027,1485) (2028,1486) (2028,1859) (2027,1860) (1759,1860) (1758,1859) (1758,1293) (1759,1292)
                    var pointSplit = justPoints.Split(new char[] { ')' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(var pointString in pointSplit)
                    {
                        // clear the opening paranthesis
                        var cleanPoint = pointString.Replace("(", "").Trim();
                        var coordinates = cleanPoint.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var x = int.Parse(coordinates[0].Trim());
                        var y = int.Parse(coordinates[1].Trim());
                        var point = new Point(x, y);
                        pointList.Add(point);
                    }
                    matchingTerritory.PlacementLocations = pointList;
                }
                else
                {
                    Debug.WriteLine("Missing Territory with name '" + territoryName + "'");
                }
            }
        }


        /// <summary>
        /// This will load from the polygons.txt file
        /// </summary>
        /// <returns></returns>
        private async Task LoadPolygons()
        {
            var configFilePath = "ms-appx:///Game/" + scenario + "/polygons.txt";
            Uri dataUri = new Uri(configFilePath);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var polygonTextFile = await FileIO.ReadTextAsync(file);

            var allTerritoryPolygons = polygonTextFile.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

            Debug.WriteLine("Polygon Count: " + allTerritoryPolygons.Length);

            //East Compass Sea Zone  <  (1884,1292) (1961,1292) (1962,1293) (1962,1485) (2027,1485) (2028,1486) (2028,1859) (2027,1860) (1759,1860) (1758,1859) (1758,1293) (1759,1292) > 

            foreach(var polygon in allTerritoryPolygons)
            {
                var endOfTerritoryName = polygon.IndexOf("<");
                //var endOfPoints = polygon.IndexOf(">");
                var territoryName = polygon.Substring(0, endOfTerritoryName).Trim();
                Debug.WriteLine("Parsing Polygon for " + territoryName);

                var startOfPointsIndex = endOfTerritoryName + 1;
                var lengthBeforePoints = startOfPointsIndex;
                //var lengthAfterPoints = polygon.Length - endOfPoints;
                //var lengthOfPoints = polygon.Length - lengthBeforePoints - lengthAfterPoints;
                var lengthOfPoints = polygon.Length - lengthBeforePoints;

                Debug.WriteLine("Length of Points: " + lengthOfPoints);

                var onlyPoints = polygon.Substring(startOfPointsIndex, lengthOfPoints).Trim();

                Debug.WriteLine("String containing only points: " + onlyPoints);

                var figureSplit = onlyPoints.Split(new char[] { '>' }, StringSplitOptions.RemoveEmptyEntries);

                foreach(var figureString in figureSplit)
                {
                    var fig = new Figure();
                    Debug.WriteLine("Isolated Figure: " + figureString);
                    var figureStringCleaned = figureString.Replace("<", "").Trim();
                    Point startPoint = null;
                    var pointList = new List<Point>();
                    //(1884,1292) (1961,1292) (1962,1293) (1962,1485) (2027,1485) (2028,1486) (2028,1859) (2027,1860) (1759,1860) (1758,1859) (1758,1293) (1759,1292)
                    var pointSplit = figureStringCleaned.Split(new char[] { ')' }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (var pointString in pointSplit)
                    {
                        // clear the opening paranthesis
                        var cleanPoint = pointString.Replace("(", "").Trim();
                        var coordinates = cleanPoint.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        var x = int.Parse(coordinates[0].Trim());
                        var y = int.Parse(coordinates[1].Trim());
                        var point = new Point(x, y);
                        if (startPoint == null)
                        {
                            startPoint = point;
                        }
                        pointList.Add(point);
                    }
                    pointList.Add(startPoint);

                    var matchingTerritory = this.Map.Territories.Where(f => f.Name == territoryName).FirstOrDefault();

                    if (matchingTerritory != null)
                    {
                        fig.Points = pointList;
                        fig.StartPoint = startPoint;
                        matchingTerritory.Figures.Add(fig);
                    }
                    else
                    {
                        Debug.WriteLine("Missing Territory with name '" + territoryName + "'");
                    }
                }

            }
        }

        private Player GeneratePlayer(string name, bool isOptional)
        {
            var newPlayer = new Player();
            newPlayer.Name = name;
            newPlayer.IsOptional = isOptional;
            var playerColorKey = "color." + name;
            if (this.Map.Properties.ContainsKey(playerColorKey))
            {
                newPlayer.Color = this.Map.Properties[playerColorKey];
            }

            newPlayer.Resources = new List<Resource>();

            return newPlayer;
        }

        /// <summary>
        /// Pre-condition: map.properties has been loaded using 'LoadMap()'
        /// 
        /// This will load the XML config file fo r the given game (e.g. classic.xml, classic_3rd_edition.xml, iron_blitz.xml, etc.)
        /// 
        /// </summary>
        /// <param name="configFile"></param>
        /// <returns></returns>
        private async Task LoadGameConfig(string configFile)
        {
            var configFilePath = "ms-appx:///Game/" + scenario + "/games/" + configFile;
            Uri dataUri = new Uri(configFilePath);
            StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(dataUri);
            var gamesFolder = await file.GetParentAsync();
            var scenarioFolder = await gamesFolder.GetParentAsync();
            var configFileText = await FileIO.ReadTextAsync(file);
            var configDoc = XDocument.Parse(configFileText);
            var gameElement = configDoc.Element("game");

            // This is complete
            #region Map
            var mapElement = gameElement.Element("map");
            var territoryElements = mapElement.Elements("territory");
            foreach (var territoryElement in territoryElements)
            {
                Territory t = new Territory();
                t.Name = territoryElement.Attribute("name").Value;
                var waterAtt = territoryElement.Attribute("water");
                if(waterAtt != null)
                {
                    var waterValue = waterAtt.Value;
                    var isWater = false;
                    bool.TryParse(waterValue, out isWater);
                    t.IsWater = isWater;
                }

                Map.Territories.Add(t);
            }
            var connectionElements = mapElement.Elements("connection");
            foreach (var connectionElement in connectionElements)
            {
                var t1Value = connectionElement.Attribute("t1").Value;
                var t2Value = connectionElement.Attribute("t2").Value;

                var t1Territory = Map.Territories.Where(f => f.Name == t1Value).FirstOrDefault();
                var t2Territory = Map.Territories.Where(f => f.Name == t2Value).FirstOrDefault();

                if (t1Territory != null & t2Territory != null)
                {
                    // then we are good
                    // now create reciprocal connections between the two territories
                    t1Territory.Connections.Add(t2Territory);
                    t2Territory.Connections.Add(t1Territory);
                }
                else
                {
                    Debug.WriteLine("Unable to make connection between " + t1Value + " and " + t2Value);
                }
            }

            #endregion

            // polygons.txt
            await this.LoadPolygons();
            // centers.txt
            await this.LoadCenters();
            // place.txt
            await this.LoadPlacementLocations();

            #region Resource Type List
            var resourceListElement = gameElement.Element("resourceList");

            this.ResourceTypes = new List<ResourceType>();
            var resourceElements = resourceListElement.Elements("resource");
            foreach (var resourceElement in resourceElements)
            {
                var resourceItem = new ResourceType();
                resourceItem.Name = resourceElement.Attribute("name").Value;
                this.ResourceTypes.Add(resourceItem);
            }
            #endregion

            // this is complete
            #region Player List
            var playerListElement = gameElement.Element("playerList");
            #region Players
            foreach (var playerElement in playerListElement.Elements("player"))
            {
                var playerName = playerElement.Attribute("name").Value;
                var isOptional = bool.Parse(playerElement.Attribute("optional").Value);
                var newPlayer = GeneratePlayer(playerName, isOptional);

                this.Players.Add(newPlayer);
            }
            var waterPlayer = GeneratePlayer("Water", false);
            this.Players.Add(waterPlayer);

            var neutralPlayer = GeneratePlayer("Neutral", false);
            this.Players.Add(neutralPlayer);

            #endregion
            #region Alliances
            this.Alliances = new List<Alliance>();

            foreach (var allianceElement in playerListElement.Elements("alliance"))
            {
                var allianceName = allianceElement.Attribute("alliance").Value;
                var matchingAlliance = this.Alliances.Where(f => f.Name == allianceName).FirstOrDefault();
                if(matchingAlliance == null)
                {
                    var newAlliance = new Alliance();
                    newAlliance.Name = allianceName;
                    this.Alliances.Add(newAlliance);

                    Debug.WriteLine("Creating a new alliance called '" + allianceName + "'");

                    matchingAlliance = newAlliance;
                }

                var playerName = allianceElement.Attribute("player").Value;
                var matchingPlayer = this.Players.Where(f => f.Name == playerName).FirstOrDefault();
                if(matchingPlayer != null)
                {
                    matchingAlliance.Players.Add(matchingPlayer);
                    Debug.WriteLine("Adding " + playerName + " to the " + allianceName + " alliance.");
                }
            }
            #endregion
            #endregion

            // this is complete
            #region Unit List
            this.UnitTypes = new List<UnitType>();
            var unitList = gameElement.Element("unitList");
            foreach(var unitElement in unitList.Elements("unit"))
            {
                var unitName = unitElement.Attribute("name").Value;
                var newUnitType = new UnitType();
                newUnitType.Name = unitName;

                this.UnitTypes.Add(newUnitType);
            }
            #endregion

            #region Game Play
            var gamePlay = gameElement.Element("gamePlay");

            #region Delegates
            this.SequenceDelegates = new List<SequenceStepType>();

            foreach (var unitElement in gamePlay.Elements("delegate"))
            {
                var delegateName = unitElement.Attribute("name").Value;
                var javaClassName = unitElement.Attribute("javaClass").Value;
                var displayName = unitElement.Attribute("display").Value;

                var newEventTypeDelegate = new SequenceStepType();
                newEventTypeDelegate.Name = delegateName;
                //TODO: use javaClass
                //TODO: use display

                this.SequenceDelegates.Add(newEventTypeDelegate);
            }
            #endregion

            this.Sequence = new List<SequenceStep>();

            var sequenceStepList = gamePlay.Element("sequence");
            foreach (var stepElement in sequenceStepList.Elements("step"))
            {
                var newSequenceStep = new SequenceStep();

                #region Step Name
                var stepName = stepElement.Attribute("name").Value;
                newSequenceStep.Name = stepName;
                #endregion
                #region Delegate
                var delegateName = stepElement.Attribute("delegate").Value;
                newSequenceStep.Delegate = this.SequenceDelegates.Where(f => f.Name == delegateName).FirstOrDefault();
                #endregion
                #region Player
                var playerAtt = stepElement.Attribute("player");
                if (playerAtt != null)
                {
                    // apply to ALL players?!
                    var playerName = playerAtt.Value;
                    newSequenceStep.Player = this.Players.Where(f => f.Name == playerName).FirstOrDefault();
                }
                #endregion
                #region Display
                var displayAtt = stepElement.Attribute("display");
                if (displayAtt != null)
                {
                    newSequenceStep.Display = displayAtt.Value;
                }
                #endregion
                #region Max Run Count
                var maxRunCountAtt = stepElement.Attribute("maxRunCount");
                int? maxRunCount = null;
                if(maxRunCountAtt != null)
                {
                    int maxRunCountVal;
                    var runCountText = stepElement.Attribute("maxRunCount").Value;
                    int.TryParse(runCountText, out maxRunCountVal);
                    maxRunCount = maxRunCountVal;
                    newSequenceStep.MaxRunCount = maxRunCount;
                }
                #endregion

                this.Sequence.Add(newSequenceStep);
            }

            #endregion

            #region Production

            #region Production Rules
            this.ProductionRules = new List<ProductionRule>();

            var productionElement = gameElement.Element("production");
            foreach (var ruleElement in productionElement.Elements("productionRule"))
            {
                var ruleName = ruleElement.Attribute("name").Value;
                var newProductionRule = new ProductionRule();
                newProductionRule.Name = ruleName;
                //initialize costs collection

                #region Costs
                newProductionRule.Costs = new List<ProductionRuleCost>();
                foreach (var costElement in ruleElement.Elements("cost"))
                {
                    var newCost = new ProductionRuleCost();
                    #region Resource Type
                    var resourceTypeName = costElement.Attribute("resource").Value;
                    newCost.Resource = this.ResourceTypes.Where(f => f.Name == resourceTypeName).FirstOrDefault();
                    #endregion
                    #region Quantity
                    var quantityText = costElement.Attribute("quantity").Value;
                    int quantity;
                    int.TryParse(quantityText, out quantity);
                    newCost.Quantity = quantity;
                    #endregion

                    // add to new production rule
                    newProductionRule.Costs.Add(newCost);
                }
                #endregion

                #region Results
                newProductionRule.Results = new List<ProductionRuleResult>();
                foreach (var resultElement in ruleElement.Elements("result"))
                {
                    var newResult = new ProductionRuleResult();

                    #region Unit Type
                    var unitTypeName = resultElement.Attribute("resourceOrUnit").Value;
                    newResult.UnitType = this.UnitTypes.Where(f => f.Name == unitTypeName).FirstOrDefault();
                    #endregion
                    #region Quantity
                    var quantityText = resultElement.Attribute("quantity").Value;
                    int quantity;
                    int.TryParse(quantityText, out quantity);
                    newResult.Quantity = quantity;
                    #endregion

                    // add to new production rule
                    newProductionRule.Results.Add(newResult);
                }
                #endregion

                this.ProductionRules.Add(newProductionRule);
            }
            #endregion

            #region Production Frontiers
            this.ProductionFrontiers = new List<ProductionFrontier>();
            foreach (var productionFrontier in productionElement.Elements("productionFrontier"))
            {
                var frontierName = productionFrontier.Attribute("name").Value;

                var newFrontier = new ProductionFrontier();
                newFrontier.Name = frontierName;
                newFrontier.Rules = new List<ProductionRule>();

                foreach (var frontierRule in productionFrontier.Elements("frontierRules"))
                {
                    var ruleName = frontierRule.Attribute("name").Value;
                    var rule = this.ProductionRules.Where(f => f.Name == ruleName).FirstOrDefault();
                    newFrontier.Rules.Add(rule);
                }
                this.ProductionFrontiers.Add(newFrontier);
            }
            #endregion

            #region Player Production
            foreach (var productionFrontier in productionElement.Elements("playerProduction"))
            {
                var playerName = productionFrontier.Attribute("player").Value;
                var defaultFrontierName = productionFrontier.Attribute("frontier").Value;

                // TODO: set the player's default frontier
                var player = this.Players.Where(f => f.Name == playerName).FirstOrDefault();
                player.ProductionFrontier = this.ProductionFrontiers.Where(f => f.Name == defaultFrontierName).FirstOrDefault();
            }
            #endregion

            #endregion

            #region Attachment List
            // TODO: do this

            /* attachment types
             *  - techAttatchment, player
             *  - unitAttachment, unitType
             *  - territoryAttachment, territory
             *  - 
             * */
            var attachmentListElement = gameElement.Element("attatchmentList");
            foreach (var attachmentElement in attachmentListElement.Elements("attatchment"))
            {
                /*<attatchment 
                        name ="techAttatchment" 
                        attatchTo="Japanese" 
                        javaClass="games.strategy.triplea.attatchments.TechAttachment" 
                        type = "player"
                        >
                */
                var attachmentName = attachmentElement.Attribute("name").Value;
                var attachTo = attachmentElement.Attribute("attatchTo").Value;
                var attachmentStrategyClass = attachmentElement.Attribute("javaClass").Value;
                var javaAttachmentTypeName = attachmentElement.Attribute("type").Value;

                var newAttachmentTypeName = attachmentStrategyClass.Replace("games.strategy.triplea.attatchments.", "TripleA.Attachments.");
                var attachmentType = Type.GetType(newAttachmentTypeName);
                
                if(attachmentType != null)
                {
                    Dictionary<string, string> options = new Dictionary<string, string>();
                    foreach(var optionElement in attachmentElement.Elements("option"))
                    {
                        var key = optionElement.Attribute("name").Value;
                        var val = optionElement.Attribute("value").Value;
                        options.Add(key, val);
                    }
                    var attachment = Activator.CreateInstance(attachmentType) as IAttachment;
                    attachment.Apply(attachTo, options);
                }

            }
            #endregion

            #region Detect Custom Unit Graphics
            try
            {
                var customUnitsFolder = await scenarioFolder.GetFolderAsync("units");
                this.HasCustomUnitGraphics = true;
            } 
            catch(Exception ex)
            {
                this.HasCustomUnitGraphics = false;
            }
            #endregion


            #region Initialize
            var initializeListElement = gameElement.Element("initialize");
            #region Owners
            var ownerInitializeElement = initializeListElement.Element("ownerInitialize");
            foreach(var territoryOwnerElement in ownerInitializeElement.Elements("territoryOwner"))
            {
                var territoryName = territoryOwnerElement.Attribute("territory").Value;
                var territoryOwner = territoryOwnerElement.Attribute("owner").Value;

                var ownerPlayer = this.Players.Where(f => f.Name == territoryOwner).FirstOrDefault();
                var territory = this.Map.Territories.Where(f => f.Name == territoryName).FirstOrDefault();

                territory.OriginalOwner = ownerPlayer;
                territory.CurrentOwner = ownerPlayer;
            }

            // claim water territories for the water player
            var waterTerritories = this.Map.Territories.Where(f => f.IsWater);
            foreach(var waterTerritory in waterTerritories)
            {
                waterTerritory.CurrentOwner = waterPlayer;
                waterTerritory.OriginalOwner = waterPlayer;
            }

            // claim unowned, non-water territories for the neutral player
            var unownedTerritories = from x in this.Map.Territories
                                     where x.OriginalOwner == null && !x.IsWater
                                     select x;
            foreach(var neutralTerritory in unownedTerritories)
            {
                neutralTerritory.OriginalOwner = neutralPlayer;
                neutralTerritory.CurrentOwner = neutralPlayer;
            }
            #endregion
            #region Units
            var unitInitializeElement = initializeListElement.Element("unitInitialize");
            var allUnitElements = unitInitializeElement.Elements("unitPlacement").ToList();
            foreach (var territoryOwnerElement in allUnitElements)
            {
                var newUnit = new Unit();
                var unitTypeName = territoryOwnerElement.Attribute("unitType").Value;
                var unitType = this.UnitTypes.Where(f => f.Name == unitTypeName).FirstOrDefault();
                newUnit.Type = unitType;

                var territoryName = territoryOwnerElement.Attribute("territory").Value;
                var territory = this.Map.Territories.Where(f => f.Name == territoryName).FirstOrDefault();
                newUnit.Territory = territory;

                Debug.WriteLine("Loading units in territory: " + territoryName);

                var unitQuantity = int.Parse(territoryOwnerElement.Attribute("quantity").Value);
                newUnit.Quantity = unitQuantity;

                var ownerAtt = territoryOwnerElement.Attribute("owner");
                if (ownerAtt == null)
                {
                    newUnit.Owner = this.Players.Where(f => f.Name == "Neutral").FirstOrDefault();
                }
                else
                { 
                    var unitOwnerName = ownerAtt.Value;
                    var owner = this.Players.Where(f => f.Name == unitOwnerName).FirstOrDefault();
                    newUnit.Owner = owner;
                }
                
                territory.AddUnits(newUnit);
            }
            #endregion
            #region Resources
            var resourceInitializeElement = initializeListElement.Element("resourceInitialize");
            foreach (var resourceElement in resourceInitializeElement.Elements("resourceGiven"))
            {
                var playerName = resourceElement.Attribute("player").Value;
                var player = this.Players.Where(f => f.Name == playerName).FirstOrDefault();

                var resourceTypeName = resourceElement.Attribute("resource").Value;
                var resourceType = this.ResourceTypes.Where(f => f.Name == resourceTypeName).FirstOrDefault();

                var quantityText = resourceElement.Attribute("quantity").Value;
                int quantity;
                int.TryParse(quantityText, out quantity);

                var newResource = new Resource();
                newResource.Type = resourceType;
                newResource.Quantity = quantity;
                player.Resources.Add(newResource);
            }
            #endregion
            #endregion

            #region Property List
            this.Settings = new List<GameSetting>();
            var propertyList = gameElement.Element("propertyList");
            foreach (var propertyElement in propertyList.Elements("property"))
            {
                var newGameSetting = new GameSetting();

                var propertyName = propertyElement.Attribute("name").Value;
                newGameSetting.Name = propertyName;

                var valAtt = propertyElement.Attribute("value");
                if(valAtt != null)
                {
                    var valueText = propertyElement.Attribute("value").Value;
                    newGameSetting.Value = valueText;
                }

                var editableAtt = propertyElement.Attribute("editable");
                if(editableAtt != null)
                {
                    var isEditable = bool.Parse(propertyElement.Attribute("editable").Value);
                    newGameSetting.IsEditable = isEditable;
                }

                var stringElement = propertyElement.Element("string");
                var boolElement = propertyElement.Element("boolean");
                var numberElement = propertyElement.Element("number");

                if (stringElement != null)
                {
                    newGameSetting.FieldType = "string";
                }
                else if (boolElement != null)
                {
                    newGameSetting.FieldType = "boolean";
                }
                else if(numberElement != null)
                {
                    newGameSetting.FieldType = "number";
                }

                this.Settings.Add(newGameSetting);
            }
            #endregion
            
            await this.LoadCapitols();

            Debug.WriteLine("Territory Count: " + Map.Territories.Count);
        }
    }
}