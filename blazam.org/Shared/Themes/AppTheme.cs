using BLAZAM.Helpers;
using System.Drawing;

namespace BLAZAM.Themes
{
    public class AppTheme : ApplicationTheme
    {
        public AppTheme()
        {



            _name = "Blue";


            pallete = new()
            {
                TextPrimary = "#050505",
                TextSecondary = Color.SlateGray.ToHex(),
                ActionDefault = "#9aafc3",                 
                HoverOpacity = 0,
                Surface = Color.WhiteSmoke.ToHex(),
                DarkContrastText = Color.WhiteSmoke.ToHex(),
                AppbarBackground = "#3D4047",
                DrawerBackground = "#001529",
                DrawerText = Color.WhiteSmoke.ToHex(),
                Background = "#efefef",
                //_textDark = "#001529",
                Dark = "#001529",
                Primary = "#7196D9",
                Secondary = "#0c13a7",
                Info = "#46a9ef",
                Success = Color.ForestGreen.ToHex(),
                Warning = "#ff9900",
                Error = Color.Red.ToHex(),
                //_body = Color.LightGray.ToHex(),
                TextDisabled = Color.DarkGray.ToHex(),
                White = Color.White.ToHex(),
            };

            darkPallete = new()
            {
                TextPrimary = "#050505",
                TextSecondary = Color.SlateGray.ToHex(),
                ActionDefault = "#9aafc3",
                HoverOpacity = 0,
                Surface = Color.WhiteSmoke.ToHex(),
                DarkContrastText = Color.WhiteSmoke.ToHex(),
                AppbarBackground = "#3D4047",
                DrawerBackground = "#001529",
                DrawerText = Color.WhiteSmoke.ToHex(),
                Background = "#efefef",
                //_textDark = "#001529",
                Dark = "#001529",
                Primary = "#7196D9",
                Secondary = "#0c13a7",
                Info = "#46a9ef",
                Success = Color.ForestGreen.ToHex(),
                Warning = "#ff9900",
                Error = Color.Red.ToHex(),
                //_body = Color.LightGray.ToHex(),
                TextDisabled = Color.DarkGray.ToHex(),
                White = Color.White.ToHex(),
            };

        }
    }
}
