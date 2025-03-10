using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("OldMusicBox.ePUAP.Client")]
[assembly: AssemblyDescription("Independent .NET ePUAP Client")]
[assembly: AssemblyProduct("OldMusicBox.ePUAP.Client")]
[assembly: AssemblyCopyright("Copyright © 2020-2024 Wiktor Zychla")]

[assembly: ComVisible(false)]
[assembly: Guid("7119a873-9a63-4d6d-8aeb-41c60397e2fc")]
[assembly: AssemblyVersion("1.25.03.0")]

// 25.03.0
// * od 07-03-2025 sygnatura podpisu zwracana w TpSigning5Response ma "stary" format,
//   taki jaki miała sygnatura z TpSigningResponse (PodpisZP zamiast EPSignature!)
//   krytyczna zmiana po stronie serwera. brawo!


// 24.03.0
// * dodano odczyt certyfikatu podpisu kwalifikowanego z dokumentów zwracanych z GetSignedDocument (a nie jak do tej pory
//   że odczytywana jest tylko sygnatura podpisu profilu zaufanego)

// przyjęto schemat wersjonowania X.YY.ZZ.T

// X  - duża wersja (1)
// YY - rok
// ZZ - miesiąc
// T  - wydanie w miesiącu