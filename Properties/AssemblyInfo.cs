using System.Reflection;
using MelonLoader;

[assembly: AssemblyTitle(RealisticThrusters.BuildInfo.Description)]
[assembly: AssemblyDescription(RealisticThrusters.BuildInfo.Description)]
[assembly: AssemblyCompany(RealisticThrusters.BuildInfo.Company)]
[assembly: AssemblyProduct(RealisticThrusters.BuildInfo.Name)]
[assembly: AssemblyCopyright("Created by " + RealisticThrusters.BuildInfo.Author)]
[assembly: AssemblyTrademark(RealisticThrusters.BuildInfo.Company)]
[assembly: AssemblyVersion(RealisticThrusters.BuildInfo.Version)]
[assembly: AssemblyFileVersion(RealisticThrusters.BuildInfo.Version)]
[assembly: MelonInfo(typeof(RealisticThrusters.RealisticThrusters), RealisticThrusters.BuildInfo.Name, RealisticThrusters.BuildInfo.Version, RealisticThrusters.BuildInfo.Author, RealisticThrusters.BuildInfo.DownloadLink)]
[assembly: MelonColor()]

// Create and Setup a MelonGame Attribute to mark a Melon as Universal or Compatible with specific Games.
// If no MelonGame Attribute is found or any of the Values for any MelonGame Attribute on the Melon is null or empty it will be assumed the Melon is Universal.
// Values for MelonGame Attribute can be found in the Game's app.info file or printed at the top of every log directly beneath the Unity version.
[assembly: MelonGame("CuriousOwlGames", "StellarDrive")]