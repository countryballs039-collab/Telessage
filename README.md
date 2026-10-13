# Telessage Desktop

Proyecto de escritorio para Windows basado en C# Windows Forms + Microsoft Edge WebView2.

## Compilación automática

Cada push a `main` y cada ejecución manual del workflow **Build Telessage Desktop** compilan la aplicación en un runner de Windows y publican un artefacto descargable durante 14 días.

1. Abre la pestaña **Actions** del repositorio.
2. Entra en **Build Telessage Desktop**.
3. Abre la ejecución más reciente.
4. Descarga el artefacto **Telessage-Desktop-Windows**.

## Requisito pendiente

`Telessage/index.html` es actualmente un marcador temporal. Antes de usar la app como mensajero, reemplázalo por el HTML real de Telessage. El ejecutable no implementa por sí mismo backend, autenticación ni llamadas: esas funciones deben existir y probarse en la aplicación web.

El proyecto apunta a .NET Framework 4.8 y fija Microsoft.Web.WebView2 1.0.1518.46 como SDK. Para Windows 8.1, utiliza un WebView2 Runtime compatible con ese sistema; Windows 8.1 está fuera de soporte y no se garantiza el funcionamiento en todos los equipos.
