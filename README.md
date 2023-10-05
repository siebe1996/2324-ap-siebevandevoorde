# 2324-AP-SiebeVandeVoorde

## Maze Generator

Dit is een leerproject voor applied programming. Het genereert doolhoven op 4 verschillende manieren.

### Overzicht van Projectstructuur

Deze applicatie volgt een model met drie lagen en afhankelijkheidsinjectie om deze lagen effectief met elkaar te verbinden. Hier is een uitsplitsing van de belangrijkste componenten:

#### App.xaml.cs (AppRoot - Klassebibliotheek)

- Het bestand `App.xaml.cs` binnen de klassebibliotheek `AppRoot` dient als het startpunt van onze applicatie.
- De opstartfunctie in `App.xaml.cs` is waar we diverse componenten initialiseren door hun respectievelijke fabrieken aan te roepen.
- Eerst roepen we de fabriek aan in de DataLayer, wat betekent dat we gegevens uit CSV-bestanden lezen.
- Vervolgens roepen we fabrieken aan in de LogicLayer om de overige drie scenario's te beheren.

#### DataLayer (Klassebibliotheek)

- De DataLayer bevat componenten die verantwoordelijk zijn voor gegevensgerelateerde bewerkingen, inclusief het lezen van gegevens uit CSV-bestanden.

#### LogicLayer (Klassebibliotheek)

- De LogicLayer is verantwoordelijk voor de kernlogica van onze applicatie.
- Het herbergt fabrieken voor het maken van instanties van klassen die specifieke interfaces implementeren.
- De eerste twee scenario's delen dezelfde fabriek, omdat ze deel uitmaken van dezelfde klasse.
- Klassen in deze laag zijn ontworpen om via interfaces aan MainWindow te worden doorgegeven.

##### Klassen in LogicLayer

- **AddWallMazeGenerator** en **RemoveWallMazeGenerator** implementeren beide dezelfde interface en vertegenwoordigen gedeeld gedrag.
- **BasicMazeGenerator** volgt een andere interface omdat het input van de frontend vereist, specifiek het bestandspad.

#### PresentationLayer (MainWindow)

- MainWindow, gelegen in de PresentationLayer, fungeert als de grafische gebruikersinterface (GUI) van onze applicatie.

Deze projectstructuur volgt een goed georganiseerde scheiding van verantwoordelijkheden, waarbij elke laag zijn eigen rol vervult. Afhankelijkheidsinjectie vergemakkelijkt de verbinding tussen lagen, waardoor een modulair en onderhoudsvriendelijk ontwerp mogelijk is.

### Model

De `Maze`-klasse maakt deel uit van de `Globals.Entities`-namespace en vertegenwoordigt een doolhofstructuur in een computerprogramma. Deze klasse is ontworpen om doolhoven te creëren, beheren en manipuleren, met name voor doolhofoplossingsapplicaties of -spellen.

#### Eigenschappen

- **Width** (int): Geeft de breedte van het doolhof weer.

- **Height** (int): Geeft de hoogte van het doolhof weer.

- **WallThickness** (int): Geeft de dikte van de doolhofcellen weer; zowel wanden als gangen zijn cellen.

- **BallPosition** (Coordinate): Geeft de huidige positie van een bal binnen het doolhof weer, en kan worden ingesteld.

- **MazeGraph** (UndirectedGraph<MazeNode, Edge<MazeNode>>): Vertegenwoordigt het doolhof als een ongerichte grafiek met behulp van de QuickGraph-bibliotheek. Het bevat knooppunten (vertegenwoordigd door `MazeNode`-objecten) en randen (vertegenwoordigd door `Edge<MazeNode>`-objecten) die de connectiviteit van het doolhof definiëren.

#### Constructor

##### `Maze(int width, int height, int wallThickness)`
- Initialiseert een nieuw exemplaar van de `Maze`-klasse met de opgegeven breedte, hoogte en wanddikte.
- Gooit een `ArgumentException` als de opgegeven dimensies of wanddikte ongeldig zijn (minder dan of gelijk aan 0).

#### Methoden

##### `MoveBall(int deltaX, int deltaY)`
- Verplaatst de bal binnen het doolhof door zijn positie aan te passen op basis van de gegeven `deltaX` (verandering in de X-coördinaat) en `deltaY` (verandering in de Y-coördinaat).
- Zorgt ervoor dat de nieuwe positie binnen de grenzen van het doolhof blijft.

##### `ConnectAllNodes()`
- Legt verbindingen tussen doolhofknooppunten in de `MazeGraph` om de lay-out van het doolhof voor te stellen.
- Het doorloopt elke cel in het doolhof, verbindt knooppunten met hun aangrenzende buren en houdt rekening met open cellen ('0').

#### Gebruik

Hier is een voorbeeld van hoe de `Maze`-klasse kan worden gebruikt:

```csharp
// Creëer een nieuw doolhof met afmetingen 10x10 en een wanddikte van 1
Maze mijnDoolhof = new Maze(10, 10, 1);

// Verplaats de bal binnen het doolhof
mijnDoolhof.MoveBall(1, 0); // Verplaats de bal één stap naar rechts

// Verbind alle knooppunten in het doolhof om de lay-out voor te stellen
mijnDoolhof.ConnectAllNodes();
```

### Algoritmen

#### `AddWallMazeGenerator.cs`

##### `private void AddRandomWalls(Maze maze)`

Deze private methode is verantwoordelijk voor het toevoegen van willekeurige muren aan een opgegeven doolhof. Het gebruikt het gegeven `Maze`-object om de `MazeGraph`-eigenschap ervan aan te passen.

**Parameters:**
- `maze` (Maze): Het doolhof waaraan willekeurige muren moeten worden toegevoegd.

**Functionaliteit:**
- Eerst bepaalt het de breedte en hoogte van het doolhof.
- Vervolgens voegt het muren toe langs de boven- en onderkant van het doolhof, waardoor ze effectief worden afgesloten.
- Daarna voegt het muren toe langs de linker- en rechterranden, met uitzondering van de hoeken.
- Ten slotte voegt het willekeurig muren toe binnen het interieur van het doolhof. Het aantal toe te voegen muren wordt berekend als een fractie (30%) van het totale aantal beschikbare posities in het interieur, exclusief de rand.

**Opmerking:**
- Muren worden binnen de `MazeGraph` van het doolhof gerepresenteerd door het teken '1'.
- De plaatsing van muren langs de randen zorgt ervoor dat het doolhof begrensd blijft.

Deze methode draagt bij aan de generatie van een willekeurige doolhofstructuur, waarbij obstakels en complexiteit worden geïntroduceerd voor het navigeren in het doolhof.

#### `RemoveWallMazeGenerator.cs`

##### `private void GenerateMazeRecursive(Maze maze, int currentRow, int currentCol)`

Deze private methode genereert een doolhof recursief met behulp van een diepte-eerst algoritme.

**Parameters:**
- `maze` (Maze): Het doolhofobject om te genereren.
- `currentRow` (int): De huidige rijpositie binnen het doolhof.
- `currentCol` (int): De huidige kolompositie binnen het doolhof.

**Functionaliteit:**
- Het markeert de huidige cel als open, gerepresenteerd door het teken '0' binnen de `MazeGraph` van het `maze`-object.
- De methode genereert een willekeurige volgorde voor de richtingen (omhoog, omlaag, links, rechts) en schudt deze.
- Voor elke richting berekent het de positie van de volgende cel en controleert of het een geldige cel is binnen het doolhof.
- Als de cel geldig is, markeert het de cel tussen de huidige en volgende cel als open, bezoekt het de volgende cel recursief en gaat het door met de generatie van het doolhof.

##### `private bool IsValidCell(Maze maze, int row, int col)`

Deze private methode controleert of een cel op een opgegeven rij- en kolompositie een geldige cel is voor doolhofgeneratie.

**Parameters:**
- `maze` (Maze): Het doolhofobject om te controleren.
- `row` (int): De rijpositie van de cel.
- `col` (int): De kolompositie van de cel.

**Returns:**
- `true` als de cel geldig is (binnen de grenzen van het doolhof en gesloten), anders `false`.

##### `private void Shuffle<T>(List<T> list)`

Deze private methode schudt de elementen in een lijst met behulp van het Fisher-Yates schudalgoritme.

**Parameters:**
- `list` (List<T>): De lijst die moet worden geschud.

**Functionaliteit:**
- Het doorloopt de lijst en wisselt willekeurig elementen om een willekeurige volgorde te bereiken.
- Deze methode wordt gebruikt om de richtingen voor doolhofgeneratie te schudden, waardoor willekeurigheid in de lay-out van het doolhof wordt gegarandeerd.

Deze functies werken samen om een doolhof te genereren met behulp van een recursief algoritme, waarbij wordt gezorgd dat het doolhof zowel geldig als willekeurig is voor een leuke en uitdagende ervaring.

### Grafische gebruikersinterface

#### Basis Doolhofgenerator (BasicMazeGenerator.cs)

![Stap 1](readmeImages/BasicMazeGenerator1.jpg)
1. **Selecteer een CSV-bestand:** U kunt een CSV-bestand kiezen vanaf uw pc door de pad ervan in het tekstvak in te voeren of eenvoudigweg de map te selecteren met behulp van de knop "Browse".

![Stap 2](readmeImages/BasicMazeGenerator2.jpg)
2. **Genereer het doolhof:** Druk op de knop "Generate Char Maze" of "Generate Graph Maze". Beide opties produceren hetzelfde doolhof, maar gebruiken verschillende methoden voor de creatie ervan.

![Stap 3](readmeImages/BasicMazeGenerator3.jpg)
3. **Bekijk het resultaat:** Het resulterende doolhof wordt vanonder weergegeven.

#### Doolhofgenerator met Muren Toevoegen (AddWallMazeGenerator.cs)

![Stap 1](readmeImages/AddWallMazeGenerator1.jpg)
1. **Genereer het doolhof:** Druk op de knop "Generate Add Wall Graph Maze".

![Stap 2](readmeImages/AddWallMazeGenerator2.jpg)
2. **Bekijk het resultaat:** Het gegenereerde doolhof wordt vanonder weergegeven.

#### Doolhofgenerator met Muren Verwijderen (RemoveMazeGenerator.cs)

![Stap 1](readmeImages/RemoveWallMazeGenerator1.jpg)
1. **Genereer het doolhof:** Druk op de knop "Generate Remove Wall Graph Maze".

![Stap 2](readmeImages/RemoveWallMazeGenerator2.jpg)
2. **Bekijk het resultaat:** Het gegenereerde doolhof wordt vanonder weergegeven.

### Reflectie

Het ontwikkelen van het "Maze Generator" project was een leerzaam proces waarbij ik verschillende aspecten van softwareontwikkeling heb verkend. Hier zijn enkele belangrijke inzichten en reflecties:

1. **Drie-Lagen Architectuur**: Het project volgde een drie-lagen architectuurmodel, waarbij ik duidelijk onderscheid maak tussen de presentatielaag, logische laag en datalaag. Deze aanpak hielp om de codebase georganiseerd en onderhoudsvriendelijk te houden.

2. **Dependency Injection**: Ik heb gebruikgemaakt van dependency injection om de verschillende lagen van onze toepassing met elkaar te verbinden. Dit maakte het mogelijk om afhankelijkheden op een flexibele en gecontroleerde manier te beheren.

3. **Maze Generatie**: Het implementeren van verschillende algoritmen voor doolhofgeneratie, zoals het willekeurig toevoegen van muren en het recursief genereren van doolhoven, was uitdagend maar lonend. Ik begrijp nu hoe doolhoven dynamisch kunnen worden gecreëerd en aangepast.

4. **Grafische Gebruikersinterface (GUI)**: Het ontwikkelen van de GUI met behulp van WPF was een interessante ervaring. Ik hebb geleerd hoe we gebruikersvriendelijke interfaces kunnen ontwerpen en interactieve elementen kunnen toevoegen.
											Hoewel deze mooier kan ben ik fan van simpele GUI's met weinig opmaak. Deze zijn vaak simpler en duidelijker te begrijpe.

5. **Code Organisatie**: Het belang van goed gestructureerde en gedocumenteerde code is duidelijk geworden. Het helpt niet alleen bij het begrijpen van de code, maar het oogt ook mooier.

6. **Testen en Debuggen**: Het testen van verschillende scenario's en het debuggen van problemen was een integraal onderdeel van het ontwikkelingsproces. Ik heb geleerd hoe belangrijk het is om code grondig te testen voordat deze in productie gaat.
						   Ook het Debuggen kan handig zijn voor het begrijpen van code voorbeelden. Zeker het recursief algoritme.

