using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Themes.Default;

namespace MauiBootstrapTheme.Sample.Reactor;

class MainPage : Component
{
    public override VisualNode Render()
        => Shell(
            FlyoutItem("Controls",
                ShellContent().RenderContent(() => new ControlsPage()).Route("controls")
            ).Route("controls"),
            FlyoutItem("Inputs",
                ShellContent().RenderContent(() => new InputsPage()).Route("inputs")
            ).Route("inputs"),
            FlyoutItem("Typography",
                ShellContent().RenderContent(() => new TypographyPage()).Route("typography")
            ).Route("typography"),
            FlyoutItem("Cards",
                ShellContent().RenderContent(() => new CardsPage()).Route("cards")
            ).Route("cards"),
            FlyoutItem("Themes",
                ShellContent().RenderContent(() => new ThemesPage()).Route("themes")
            ).Route("themes")
        )
        .Title("Bootstrap Theme")
        .FlyoutBehavior(FlyoutBehavior.Flyout);
}

class ControlsPage : Component
{
    public override VisualNode Render()
        => ContentPage("Controls",
            ScrollView(
                VStack(spacing: 24,
                    // Page Header (left-aligned like Blazor)
                    VStack(spacing: 4,
                        Label("Bootstrap Controls").FontSize(32).FontAttributes(FontAttributes.Bold),
                        Label("Stock MAUI controls styled with Bootstrap 5 theme values.").FontSize(18).TextColor(Colors.Gray)
                    ),

                    // Buttons Section
                    VStack(spacing: 8,
                        Label("Buttons").FontSize(24),
                        FlexLayout(
                            Button("Primary").Primary().HeightRequest(38).Margin(0, 0, 8, 8),
                            Button("Secondary").Secondary().HeightRequest(38).Margin(0, 0, 8, 8),
                            Button("Success").Success().HeightRequest(38).Margin(0, 0, 8, 8),
                            Button("Danger").Danger().HeightRequest(38).Margin(0, 0, 8, 8),
                            Button("Warning").Warning().HeightRequest(38).Margin(0, 0, 8, 8),
                            Button("Info").Info().HeightRequest(38).Margin(0, 0, 8, 8)
                        ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                    ),

                    // Outline Buttons Section
                    VStack(spacing: 8,
                        Label("Outline Buttons").FontSize(20),
                        FlexLayout(
                            Button("Primary").Primary().Outlined().HeightRequest(38).Margin(0, 0, 8, 8),
                            Button("Secondary").Secondary().Outlined().HeightRequest(38).Margin(0, 0, 8, 8),
                            Button("Success").Success().Outlined().HeightRequest(38).Margin(0, 0, 8, 8),
                            Button("Danger").Danger().Outlined().HeightRequest(38).Margin(0, 0, 8, 8)
                        ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                    ),

                    // Button Sizes Section
                    VStack(spacing: 8,
                        Label("Button Sizes").FontSize(20),
                        FlexLayout(
                            Button("Large").Primary().Large().HeightRequest(48).Margin(0, 0, 8, 8),
                            Button("Default").Primary().HeightRequest(38).Margin(0, 0, 8, 8),
                            Button("Small").Primary().Small().HeightRequest(31).Margin(0, 0, 8, 8)
                        ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                    ),

                    // Pill Buttons Section
                    VStack(spacing: 8,
                        Label("Pill Buttons").FontSize(20),
                        FlexLayout(
                            Button("Primary Pill").Primary().Pill().HeightRequest(38).Margin(0, 0, 8, 8),
                            Button("Success Pill").Success().Pill().HeightRequest(38).Margin(0, 0, 8, 8)
                        ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                    ),

                    // Progress Section
                    VStack(spacing: 12,
                        Label("Progress").FontSize(24),
                        VStack(spacing: 4,
                            Label("Primary (75%)").FontSize(14),
                            ProgressBar().Progress(0.75).Primary().BootstrapHeight()
                        ),
                        VStack(spacing: 4,
                            Label("Success (50%)").FontSize(14),
                            ProgressBar().Progress(0.5).Success().BootstrapHeight()
                        ),
                        VStack(spacing: 4,
                            Label("Danger (25%)").FontSize(14),
                            ProgressBar().Progress(0.25).Danger().BootstrapHeight()
                        )
                    ),

                    // Spinners Section
                    VStack(spacing: 8,
                        Label("Spinners").FontSize(24),
                        HStack(spacing: 16,
                            ActivityIndicator().IsRunning(true).Primary(),
                            ActivityIndicator().IsRunning(true).Success(),
                            ActivityIndicator().IsRunning(true).Danger()
                        )
                    )
                ).Padding(20)
            )
        );
}

class InputsPage : Component
{
    public override VisualNode Render()
        => ContentPage("Inputs",
            ScrollView(
                VStack(spacing: 24,
                    // Page Header
                    VStack(spacing: 4,
                        Label("Input Controls").FontSize(32).FontAttributes(FontAttributes.Bold),
                        Label("Form inputs with Bootstrap styling").FontSize(18).TextColor(Colors.Gray)
                    ),

                    // Text Inputs
                    VStack(spacing: 12,
                        Label("Text Inputs").FontSize(24),
                        VStack(spacing: 4,
                            Label("Default Input").FontSize(14),
                            Entry().Placeholder("Enter text...").BootstrapHeight()
                        ),
                        VStack(spacing: 4,
                            Label("Large Input").FontSize(14),
                            Entry().Placeholder("Large input").Large().HeightRequest(48)
                        ),
                        VStack(spacing: 4,
                            Label("Small Input").FontSize(14),
                            Entry().Placeholder("Small input").Small().HeightRequest(31)
                        )
                    ),

                    // Textarea
                    VStack(spacing: 12,
                        Label("Textarea").FontSize(24),
                        VStack(spacing: 4,
                            Label("Default Textarea").FontSize(14),
                            Editor().Placeholder("Enter multi-line text...").HeightRequest(100)
                        )
                    ),

                    // Select
                    VStack(spacing: 12,
                        Label("Select").FontSize(24),
                        VStack(spacing: 4,
                            Label("Default Select").FontSize(14),
                            Picker().Title("Choose an option...").ItemsSource(new[] { "Option 1", "Option 2", "Option 3" })
                        )
                    ),

                    // Date & Time
                    VStack(spacing: 12,
                        Label("Date & Time").FontSize(24),
                        HStack(spacing: 20,
                            VStack(spacing: 4,
                                Label("Date").FontSize(14),
                                DatePicker()
                            ).HFill(),
                            VStack(spacing: 4,
                                Label("Time").FontSize(14),
                                TimePicker()
                            ).HFill()
                        )
                    ),

                    // Checkboxes & Switches
                    VStack(spacing: 12,
                        Label("Checkboxes & Switches").FontSize(24),
                        HStack(spacing: 8,
                            CheckBox().IsChecked(true).Primary(),
                            Label("Default checkbox").VCenter()
                        ),
                        HStack(spacing: 8,
                            CheckBox().Primary(),
                            Label("Unchecked checkbox").VCenter()
                        ),
                        HStack(spacing: 8,
                            Switch().IsToggled(true).Primary(),
                            Label("Enabled switch").VCenter()
                        ),
                        HStack(spacing: 8,
                            Switch().Primary(),
                            Label("Disabled switch").VCenter()
                        )
                    ),

                    // Range Slider
                    VStack(spacing: 12,
                        Label("Range Slider").FontSize(24),
                        VStack(spacing: 4,
                            Label("Default Range").FontSize(14),
                            Slider().Minimum(0).Maximum(100).Value(50).Primary()
                        )
                    )
                ).Padding(20)
            )
        );
}

class TypographyPage : Component
{
    public override VisualNode Render()
        => ContentPage("Typography",
            ScrollView(
                VStack(spacing: 24,
                    // Page Header
                    VStack(spacing: 4,
                        Label("Typography").FontSize(32).FontAttributes(FontAttributes.Bold),
                        Label("Bootstrap typography utilities").FontSize(18).TextColor(Colors.Gray)
                    ),

                    // Headings
                    VStack(spacing: 12,
                        Label("Headings").FontSize(24),
                        Border(
                            VStack(spacing: 8,
                                Label("Heading 1 (h1)").H1(),
                                Label("Heading 2 (h2)").H2(),
                                Label("Heading 3 (h3)").H3(),
                                Label("Heading 4 (h4)").H4(),
                                Label("Heading 5 (h5)").H5(),
                                Label("Heading 6 (h6)").H6()
                            ).Padding(16)
                        ).ShadowSm()
                    ),

                    // Text Styles
                    VStack(spacing: 12,
                        Label("Text Styles").FontSize(24),
                        Border(
                            VStack(spacing: 8,
                                Label("Lead text - larger and lighter for introductions.").Lead(),
                                Label("Normal body text. Lorem ipsum dolor sit amet."),
                                Label("Small text for fine print.").TextStyle(BootstrapTextStyle.Small),
                                Label("Muted text for secondary content.").Muted()
                            ).Padding(16)
                        ).ShadowSm()
                    ),

                    // Text Colors
                    VStack(spacing: 12,
                        Label("Text Colors").FontSize(24),
                        Border(
                            VStack(spacing: 4,
                                Label("Primary text color").TextColor(BootstrapVariant.Primary),
                                Label("Secondary text color").TextColor(BootstrapVariant.Secondary),
                                Label("Success text color").TextColor(BootstrapVariant.Success),
                                Label("Danger text color").TextColor(BootstrapVariant.Danger),
                                Label("Warning text color").TextColor(BootstrapVariant.Warning),
                                Label("Info text color").TextColor(BootstrapVariant.Info)
                            ).Padding(16)
                        ).ShadowSm()
                    ),

                    // Badges
                    VStack(spacing: 12,
                        Label("Badges").FontSize(24),
                        Border(
                            VStack(spacing: 12,
                                FlexLayout(
                                    Label("Primary").Badge(BootstrapVariant.Primary).Margin(0, 0, 8, 8),
                                    Label("Secondary").Badge(BootstrapVariant.Secondary).Margin(0, 0, 8, 8),
                                    Label("Success").Badge(BootstrapVariant.Success).Margin(0, 0, 8, 8),
                                    Label("Danger").Badge(BootstrapVariant.Danger).Margin(0, 0, 8, 8),
                                    Label("Warning").Badge(BootstrapVariant.Warning).Margin(0, 0, 8, 8),
                                    Label("Info").Badge(BootstrapVariant.Info).Margin(0, 0, 8, 8)
                                ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap),
                                Label("Pill Badges").FontSize(18),
                                FlexLayout(
                                    Label("Primary").Badge(BootstrapVariant.Primary).Margin(0, 0, 8, 8),
                                    Label("Success").Badge(BootstrapVariant.Success).Margin(0, 0, 8, 8),
                                    Label("Danger").Badge(BootstrapVariant.Danger).Margin(0, 0, 8, 8)
                                ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                            ).Padding(16)
                        ).ShadowSm()
                    )
                ).Padding(20)
            )
        );
}

class CardsPage : Component
{
    public override VisualNode Render()
        => ContentPage("Cards",
            ScrollView(
                VStack(spacing: 24,
                    // Page Header
                    VStack(spacing: 4,
                        Label("Cards & Containers").FontSize(32).FontAttributes(FontAttributes.Bold),
                        Label("Bootstrap card components with shadows").FontSize(18).TextColor(Colors.Gray)
                    ),

                    // Shadow Variants
                    VStack(spacing: 12,
                        Label("Shadow Variants").FontSize(24),
                        Grid("*,*", "*",
                            Border(
                                VStack(
                                    Label("No Shadow").FontAttributes(FontAttributes.Bold),
                                    Label("Default card without shadow").TextColor(Colors.Gray)
                                ).Padding(16)
                            ).GridColumn(0),
                            Border(
                                VStack(
                                    Label("Small Shadow").FontAttributes(FontAttributes.Bold),
                                    Label("shadow-sm class").TextColor(Colors.Gray)
                                ).Padding(16)
                            ).ShadowSm().GridColumn(1)
                        ).ColumnSpacing(16),
                        Grid("*,*", "*",
                            Border(
                                VStack(
                                    Label("Default Shadow").FontAttributes(FontAttributes.Bold),
                                    Label("shadow class").TextColor(Colors.Gray)
                                ).Padding(16)
                            ).ShadowMd().GridColumn(0),
                            Border(
                                VStack(
                                    Label("Large Shadow").FontAttributes(FontAttributes.Bold),
                                    Label("shadow-lg class").TextColor(Colors.Gray)
                                ).Padding(16)
                            ).ShadowLg().GridColumn(1)
                        ).ColumnSpacing(16)
                    ),

                    // Border Color Variants
                    VStack(spacing: 12,
                        Label("Border Color Variants").FontSize(24),
                        FlexLayout(
                            Border(Label("Primary").HCenter().VCenter()).Primary().Padding(16).WidthRequest(100).HeightRequest(60).Margin(0, 0, 8, 8),
                            Border(Label("Success").HCenter().VCenter()).Success().Padding(16).WidthRequest(100).HeightRequest(60).Margin(0, 0, 8, 8),
                            Border(Label("Danger").HCenter().VCenter()).Danger().Padding(16).WidthRequest(100).HeightRequest(60).Margin(0, 0, 8, 8),
                            Border(Label("Warning").HCenter().VCenter()).Warning().Padding(16).WidthRequest(100).HeightRequest(60).Margin(0, 0, 8, 8),
                            Border(Label("Info").HCenter().VCenter()).Info().Padding(16).WidthRequest(100).HeightRequest(60).Margin(0, 0, 8, 8)
                        ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                    ),

                    // Background Variants
                    VStack(spacing: 12,
                        Label("Background Variants").FontSize(24),
                        Border(
                            VStack(
                                Label("Primary Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                                Label("text-bg-primary").TextColor(Colors.White)
                            ).Padding(16)
                        ).Background(BootstrapVariant.Primary).ShadowSm(),
                        Border(
                            VStack(
                                Label("Success Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                                Label("text-bg-success").TextColor(Colors.White)
                            ).Padding(16)
                        ).Background(BootstrapVariant.Success).ShadowSm(),
                        Border(
                            VStack(
                                Label("Dark Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                                Label("text-bg-dark").TextColor(Colors.White)
                            ).Padding(16)
                        ).Background(BootstrapVariant.Dark).ShadowSm()
                    ),

                    // Card with Header & Footer
                    VStack(spacing: 12,
                        Label("Card with Header & Footer").FontSize(24),
                        Border(
                            VStack(spacing: 0,
                                Label("Featured").Padding(12, 8).BackgroundColor(Color.FromArgb("#f8f9fa")),
                                BoxView().HeightRequest(1).Color(Colors.LightGray),
                                VStack(spacing: 8,
                                    Label("Special Title").FontSize(18).FontAttributes(FontAttributes.Bold),
                                    Label("With supporting text below as a natural lead-in to additional content."),
                                    Button("Go somewhere").Primary().HeightRequest(38)
                                ).Padding(16),
                                BoxView().HeightRequest(1).Color(Colors.LightGray),
                                Label("2 days ago").TextColor(Colors.Gray).Padding(12, 8).BackgroundColor(Color.FromArgb("#f8f9fa"))
                            )
                        ).ShadowMd()
                    )
                ).Padding(20)
            )
        );
}

class ThemesPage : Component
{
    private string _currentTheme = "Default";

    public override VisualNode Render()
    {
        var theme = BootstrapTheme.Current;
        
        return ContentPage("Themes",
            ScrollView(
                VStack(spacing: 24,
                    // Page Header
                    VStack(spacing: 4,
                        Label("Theme Switcher").FontSize(32).FontAttributes(FontAttributes.Bold),
                        Label("Switch between Bootstrap themes at runtime").FontSize(18).TextColor(Colors.Gray)
                    ),

                    // Theme Selection
                    VStack(spacing: 12,
                        Label("Select Theme").FontSize(24),
                        FlexLayout(
                            Button("Default").Primary().OnClicked(() => ApplyTheme(new DefaultTheme(), "Default")).Margin(0, 0, 8, 8),
                            Button("Darkly").Dark().OnClicked(() => ApplyTheme(new DarklyTheme(), "Darkly")).Margin(0, 0, 8, 8),
                            Button("Cyborg").Info().OnClicked(() => ApplyTheme(new CyborgTheme(), "Cyborg")).Margin(0, 0, 8, 8),
                            Button("Slate").Secondary().OnClicked(() => ApplyTheme(new SlateTheme(), "Slate")).Margin(0, 0, 8, 8),
                            Button("Flatly").Success().OnClicked(() => ApplyTheme(new FlatlyTheme(), "Flatly")).Margin(0, 0, 8, 8),
                            Button("Sketchy").Warning().OnClicked(() => ApplyTheme(new SketchyTheme(), "Sketchy")).Margin(0, 0, 8, 8),
                            Button("Vapor").Danger().OnClicked(() => ApplyTheme(new VaporTheme(), "Vapor")).Margin(0, 0, 8, 8),
                            Button("Brite").Primary().OnClicked(() => ApplyTheme(new BriteTheme(), "Brite")).Margin(0, 0, 8, 8)
                        ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap),
                        Label($"Current: {_currentTheme}").TextColor(Colors.Gray)
                    ),

                    // Preview - render controls with current theme colors directly
                    VStack(spacing: 12,
                        Label("Preview").FontSize(24),
                        Border(
                            VStack(spacing: 12,
                                Label("Theme Preview").FontSize(18).FontAttributes(FontAttributes.Bold).FontFamily(theme.FontFamily),
                                Entry().Placeholder("Sample input").HeightRequest(theme.InputMinHeight).FontFamily(theme.FontFamily),
                                FlexLayout(
                                    Button("Primary").BackgroundColor(theme.Primary).TextColor(theme.OnPrimary).HeightRequest(theme.ButtonMinHeight).CornerRadius((int)theme.CornerRadius).FontFamily(theme.FontFamily).Margin(0, 0, 8, 8),
                                    Button("Success").BackgroundColor(theme.Success).TextColor(theme.OnSuccess).HeightRequest(theme.ButtonMinHeight).CornerRadius((int)theme.CornerRadius).FontFamily(theme.FontFamily).Margin(0, 0, 8, 8),
                                    Button("Danger").BackgroundColor(theme.Danger).TextColor(theme.OnDanger).HeightRequest(theme.ButtonMinHeight).CornerRadius((int)theme.CornerRadius).FontFamily(theme.FontFamily).Margin(0, 0, 8, 8)
                                ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap),
                                FlexLayout(
                                    Button("Outline").BackgroundColor(Colors.Transparent).TextColor(theme.Primary).BorderColor(theme.Primary).BorderWidth(1).HeightRequest(theme.ButtonMinHeight).CornerRadius((int)theme.CornerRadius).FontFamily(theme.FontFamily).Margin(0, 0, 8, 8),
                                    Button("Pill").BackgroundColor(theme.Info).TextColor(theme.OnInfo).HeightRequest(theme.ButtonMinHeight).CornerRadius((int)theme.CornerRadiusPill).FontFamily(theme.FontFamily).Margin(0, 0, 8, 8)
                                ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap),
                                ProgressBar().Progress(0.65).ProgressColor(theme.Primary).HeightRequest(theme.ProgressHeight),
                                FlexLayout(
                                    Label("Badge").BackgroundColor(theme.Primary).TextColor(theme.OnPrimary).Padding(8, 4).FontFamily(theme.FontFamily).Margin(0, 0, 8, 8),
                                    Label("Success").BackgroundColor(theme.Success).TextColor(theme.OnSuccess).Padding(8, 4).FontFamily(theme.FontFamily).Margin(0, 0, 8, 8),
                                    Label("Alert").BackgroundColor(theme.Danger).TextColor(theme.OnDanger).Padding(8, 4).FontFamily(theme.FontFamily).Margin(0, 0, 8, 8)
                                ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap)
                            ).Padding(16)
                        ).Stroke(theme.GetOutline())
                    ),

                    // All Color Variants
                    VStack(spacing: 12,
                        Label("All Color Variants").FontSize(24).FontFamily(theme.FontFamily),
                        Grid("*,*", "Auto,Auto,Auto,Auto",
                            Border(Label("Primary").HCenter().TextColor(theme.OnPrimary).FontFamily(theme.FontFamily)).BackgroundColor(theme.Primary).Padding(16).GridRow(0).GridColumn(0),
                            Border(Label("Secondary").HCenter().TextColor(theme.OnSecondary).FontFamily(theme.FontFamily)).BackgroundColor(theme.Secondary).Padding(16).GridRow(0).GridColumn(1),
                            Border(Label("Success").HCenter().TextColor(theme.OnSuccess).FontFamily(theme.FontFamily)).BackgroundColor(theme.Success).Padding(16).GridRow(1).GridColumn(0),
                            Border(Label("Danger").HCenter().TextColor(theme.OnDanger).FontFamily(theme.FontFamily)).BackgroundColor(theme.Danger).Padding(16).GridRow(1).GridColumn(1),
                            Border(Label("Warning").HCenter().TextColor(theme.OnWarning).FontFamily(theme.FontFamily)).BackgroundColor(theme.Warning).Padding(16).GridRow(2).GridColumn(0),
                            Border(Label("Info").HCenter().TextColor(theme.OnInfo).FontFamily(theme.FontFamily)).BackgroundColor(theme.Info).Padding(16).GridRow(2).GridColumn(1),
                            Border(Label("Light").HCenter().TextColor(theme.OnBackground).FontFamily(theme.FontFamily)).BackgroundColor(theme.Light).Padding(16).GridRow(3).GridColumn(0),
                            Border(Label("Dark").HCenter().TextColor(theme.OnPrimary).FontFamily(theme.FontFamily)).BackgroundColor(theme.Dark).Padding(16).GridRow(3).GridColumn(1)
                        ).RowSpacing(8).ColumnSpacing(8)
                    )
                ).Padding(20)
            )
        ).BackgroundColor(theme.Background);
    }

    private void ApplyTheme(IBootstrapThemeProvider provider, string name)
    {
        _currentTheme = name;
        BootstrapTheme.SetTheme(provider.GetTheme());
        Invalidate();
    }
}
