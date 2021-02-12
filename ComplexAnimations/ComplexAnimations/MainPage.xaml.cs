using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ComplexAnimations
{
    public partial class MainPage : ContentPage
    {
        public const double MaxParticleSpeed = 6.0;
        public List<Image> Images = new List<Image>();
        public List<double> ParticleRates = new List<double>();
        bool hasAllocatedSize = false;
        public MainPage()
        {
            InitializeComponent();
            InitializeParticleSystem();
            Task.Run(BeginAnimation);
        }

        private async void BeginAnimation()
        {
            while (true)
            {
                await Task.Delay(20);
                for (int i = 0; i < Images.Count; i++)
                {
                    Image particle = Images[i] as Image;
                    double rate = ParticleRates[i];

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        particle.TranslationY += rate;

                        if (particle.TranslationY > Height)
                        {
                            particle.TranslationY -= Height + particle.Height + 100;
                        }
                    });
                    
                }
            }
        } 

        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            if (width <= 0 || height <= 0)
            {
                return;
            }

            if (hasAllocatedSize)
            {
                return;
            }
            hasAllocatedSize = true;
            Random random = new Random();
            
            for (int i = 0; i < Images.Count; i++)
            {
                Image particle = Images[i] as Image;
                double rate = ParticleRates[i];
                particle.TranslationY = random.Next(0, (int)Height);
                particle.TranslationX = width / Images.Count * i;
                particle.Scale = rate / MaxParticleSpeed;
            }
            
        }

        private void InitializeParticleSystem()
        {
            Random random = new Random();
            for (int i = 0; i < 5; i++)
            {
                var newImage = new Image
                {
                    HeightRequest = referenceImage.HeightRequest,
                    WidthRequest = referenceImage.WidthRequest,
                    Source = referenceImage.Source,
                    VerticalOptions = referenceImage.VerticalOptions,
                    HorizontalOptions = referenceImage.HorizontalOptions
                };
                particleContainer.Children.Add(newImage);
                ParticleRates.Add(random.NextDouble() * 3 + 0.1);
                Images.Add(newImage);
            }
        }
    }
}
