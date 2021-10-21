using System;
using Android.Widget;
using Android.Text;
using G = Android.Graphics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.Content;
using AndroidX.Core.Content;
using Android.Graphics;

[assembly: ExportRenderer(typeof(Xamarin.Forms.SearchBar), typeof(MyApp.Renderers.SearchBarIconColorCustomRenderer))]
namespace MyApp.Renderers
{
    public class SearchBarIconColorCustomRenderer : SearchBarRenderer
    {
        public SearchBarIconColorCustomRenderer(Context context) : base(context)
        {
            Application.Current.RequestedThemeChanged += Current_RequestedThemeChanged;
        }

        private void Current_RequestedThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            ThemeChanged();
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> args)
        {
            base.OnElementChanged(args);

            if (Control != null)
            {
                var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
                var plate = Control.FindViewById(plateId);
                plate.SetBackgroundColor(Android.Graphics.Color.Transparent);

                ThemeChanged();
            }
        }

        protected void ThemeChanged()
        {
            var iconId = Resources.GetIdentifier("android:id/search_mag_icon", null, null);
            var icon = Control.FindViewById(iconId);
            if (Application.Current.RequestedTheme == OSAppTheme.Dark)
                (icon as ImageView).SetColorFilter(Xamarin.Forms.Color.LightGray.ToAndroid(), PorterDuff.Mode.SrcIn);
        }
    }
}