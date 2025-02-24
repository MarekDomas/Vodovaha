using Android.OS;

namespace Vodovaha;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        ToggleAccelerometer();
    }

    public void ToggleAccelerometer()
    {
        if (Accelerometer.Default.IsSupported)
        {
            if (!Accelerometer.Default.IsMonitoring)
            {
                Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.Default.Start(SensorSpeed.UI);
            }
            else
            {
                Accelerometer.Default.Stop();
                Accelerometer.Default.ReadingChanged -= Accelerometer_ReadingChanged;
            }
        }
    }

    private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
    {
        var sensitivity = 50d;
        var tolerance = 0.1f;

        // Výpočet pozice bubliny na základě hodnot akcelerometru
        var bubbleX = -e.Reading.Acceleration.Y * sensitivity;
        var bubbleY = -e.Reading.Acceleration.X * sensitivity;

        // Omezení pohybu bubliny na hranice kontejneru
        bubbleX = Math.Max(-bubbleContainer.Width / 2, Math.Min(bubbleContainer.Width / 2, bubbleX));
        bubbleY = Math.Max(-bubbleContainer.Height / 2, Math.Min(bubbleContainer.Height / 2, bubbleY));

        bubble.TranslationX = bubbleX;
        bubble.TranslationY = bubbleY;

        if (Math.Abs(e.Reading.Acceleration.X) < tolerance &&
            Math.Abs(e.Reading.Acceleration.Y) < tolerance &&
            Math.Abs(e.Reading.Acceleration.Z - 1) < tolerance)
        {
            bubbleContainer.BackgroundColor = Colors.LawnGreen;
        }
        else
        {
            bubbleContainer.BackgroundColor = Colors.Red;
        }

    }
}