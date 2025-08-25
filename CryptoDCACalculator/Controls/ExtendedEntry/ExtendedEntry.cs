using CommunityToolkit.Maui.Behaviors;

namespace CryptoDCACalculator.Controls.ExtendedEntry
{
    public class ExtendedEntry : Entry
    {
        public ExtendedEntry()
        {
            FontAutoScalingEnabled = true;
        }

        private static readonly SelectAllTextBehavior _selectAllTextBehavior = new SelectAllTextBehavior();

        public static readonly BindableProperty HasBorderProperty =
            BindableProperty.Create(nameof(HasBorder), typeof(bool), typeof(ExtendedEntry),
                true);
        
        public static readonly BindableProperty PaddingProperty =
            BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ExtendedEntry), 
                new Thickness(10,0));
        
        public static readonly BindableProperty BorderWidthProperty =
            BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(ExtendedEntry), 
                DeviceInfo.Platform == DevicePlatform.iOS ? 1 : 2);
        
        public static readonly BindableProperty BorderColorProperty =
            BindableProperty.Create(nameof(BorderColor), typeof(Color),typeof(ExtendedEntry));

        public static readonly BindableProperty CornerRadiusProperty =  
            BindableProperty.Create(nameof(CornerRadius),typeof(double),typeof(ExtendedEntry),
                3.0);

        public static readonly BindableProperty SelectAllOnEditProperty =
            BindableProperty.Create(nameof(SelectAllOnEdit), typeof(bool), typeof(ExtendedEntry),
                false, propertyChanged:OnSelectAllOnEditPropertyChanged);

        public new static readonly BindableProperty BackgroundColorProperty =
            BindableProperty.Create(
                propertyName: nameof(BackgroundColor),
                returnType: typeof(Color),
                declaringType: typeof(ExtendedEntry),
                defaultValue: Colors.Transparent);

        private static void OnSelectAllOnEditPropertyChanged(BindableObject bindable, object oldvalue, object newvalue)
        {
            if (bindable is ExtendedEntry extendedEntry)
            {
                if ((bool)newvalue)
                {
                    extendedEntry.Behaviors.Add(_selectAllTextBehavior);
                }
                else
                {
                    extendedEntry.Behaviors.Remove(_selectAllTextBehavior);
                }
            }
        }


        public bool HasBorder
        {
            get => (bool) GetValue(HasBorderProperty);
            set => SetValue(HasBorderProperty, value);
        }
        
        public Thickness Padding
        {
            get => (Thickness) GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
        
        public int BorderWidth
        {
            get =>(int)GetValue(BorderWidthProperty);   
            set => SetValue(BorderWidthProperty, value);   
        }  
        
        public Color BorderColor
        {
            get => (Color) GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }
        
        public double CornerRadius  
        {
            get =>(double)GetValue(CornerRadiusProperty);   
            set => SetValue(CornerRadiusProperty, value);   
        }

        public bool SelectAllOnEdit
        {
            get =>(bool)GetValue(SelectAllOnEditProperty);
            set => SetValue(SelectAllOnEditProperty, value);
        }

        public new Color BackgroundColor
        {
            get => (Color)GetValue(BackgroundColorProperty);
            set => SetValue(BackgroundColorProperty, value);
        }
    }
}
