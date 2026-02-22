using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;

// Ogólne informacje o zestawie s¹ kontrolowane poprzez nastêpuj¹cy 
// zestaw atrybutów. Zmieñ wartoœci tych atrybutów, aby zmodyfikowaæ informacje
// powi¹zane z zestawem.
[assembly: AssemblyTitle("WpfAsyncNewtonCalculator")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("WpfAsyncNewtonCalculator")]
[assembly: AssemblyCopyright("Copyright ©  2024")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Ustawienie elementu ComVisible na wartoœæ false sprawia, ¿e typy w tym zestawie s¹ niewidoczne
// dla sk³adników COM. Jeœli potrzebny jest dostêp do typu w tym zestawie z
// COM, ustaw wartoœæ true dla atrybutu ComVisible tego typu.
[assembly: ComVisible(false)]

//Aby rozpocz¹æ kompilacjê aplikacji mo¿liwych do zlokalizowania, ustaw
//<UICulture>Kultura_u¿ywana_podczas_kodowania</UICulture> w pliku csproj
//w grupie <PropertyGroup>. Jeœli na przyk³ad jest u¿ywany jêzyk angielski (USA)
//w plikach Ÿród³owych ustaw dla elementu <UICulture> wartoœæ en-US. Nastêpnie usuñ komentarz dla
//poni¿szego atrybutu NeutralResourceLanguage. Zaktualizuj wartoœæ „en-US” w
//poni¿szej linii tak, aby dopasowaæ ustawienie UICulture w pliku projektu.

//[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.Satellite)]


[assembly: ThemeInfo(
    ResourceDictionaryLocation.None, //gdzie znajduj¹ siê s³owniki zasobów specyficznych dla motywów
                                     //(u¿ywane, jeœli nie mo¿na odnaleŸæ zasobu na stronie,
                                     // lub s³owniki zasobów aplikacji)
    ResourceDictionaryLocation.SourceAssembly //gdzie znajduje siê s³ownik zasobów ogólnych
                                              //(u¿ywane, jeœli nie mo¿na odnaleŸæ zasobu na stronie,
                                              // aplikacji lub s³owników zasobów specyficznych dla motywów)
)]


// Informacje o wersji zestawu zawieraj¹ nastêpuj¹ce cztery wartoœci:
//
//      Wersja g³ówna
//      Wersja pomocnicza
//      Numer kompilacji
//      Poprawka
//
// Mo¿esz okreœliæ wszystkie wartoœci lub u¿yæ domyœlnych numerów kompilacji i poprawki
// przy u¿yciu symbolu „*”, tak jak pokazano poni¿ej:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]
