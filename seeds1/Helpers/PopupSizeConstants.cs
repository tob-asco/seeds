namespace seeds1.Helpers;

public class PopupSizeConstants
{
    public PopupSizeConstants(IDeviceDisplay deviceDisplay)
    {
        //Medium = new(0.7 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.6 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
        //Large = new(0.9 * (deviceDisplay.MainDisplayInfo.Width / deviceDisplay.MainDisplayInfo.Density), 0.8 * (deviceDisplay.MainDisplayInfo.Height / deviceDisplay.MainDisplayInfo.Density));
        if (Application.Current != null)
        {
            Tiny = new(0.5 * Application.Current.Windows[0].Width, 0.3 * Application.Current.Windows[0].Height);
            Small = new(0.7 * Application.Current.Windows[0].Width, 0.4 * Application.Current.Windows[0].Height);
            Medium = new(0.7 * Application.Current.Windows[0].Width, 0.6 * Application.Current.Windows[0].Height);
            Large = new(0.9 * Application.Current.Windows[0].Width, 0.7 * Application.Current.Windows[0].Height);
        }
        else
        {
            Tiny = new(100, 100);
            Small = new(300, 300);
            Medium = new(400, 400);
            Large = new(500,500);
        }
    }

    public Size Tiny { get; }
    public Size Small { get; }
    public Size Medium { get; }
    public Size Large { get; }
}
