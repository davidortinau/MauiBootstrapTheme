using MauiReactor;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor;

class MainPage : Component
{
    public override VisualNode Render()
        => Shell(
            FlyoutItem("Controls",
                ShellContent()
                    .RenderContent(() => new ControlsPage())
            ),
            FlyoutItem("Typography",
                ShellContent()
                    .RenderContent(() => new TypographyPage())
            ),
            FlyoutItem("Cards",
                ShellContent()
                    .RenderContent(() => new CardsPage())
            )
        )
        .Title("Bootstrap Theme")
        .FlyoutBehavior(FlyoutBehavior.Flyout);
}

class ControlsPage : Component
{
    public override VisualNode Render()
        => ContentPage("Controls",
            ScrollView(
                VStack(spacing: 20,
                    // Header
                    Label("Bootstrap Themed Controls")
                        .H2()
                        .HCenter(),
                    
                    Label("MauiReactor fluent API demo")
                        .Lead()
                        .HCenter(),

                    // Buttons Section
                    Border(
                        VStack(spacing: 12,
                            Label("Buttons").H4(),
                            
                            HStack(spacing: 8,
                                Button("Primary").Primary(),
                                Button("Secondary").Secondary(),
                                Button("Success").Success()
                            ).HCenter(),
                            
                            HStack(spacing: 8,
                                Button("Danger").Danger(),
                                Button("Warning").Warning(),
                                Button("Info").Info()
                            ).HCenter(),
                            
                            Label("Outline Buttons").Muted(),
                            
                            HStack(spacing: 8,
                                Button("Primary")
                                    .Variant(BootstrapVariant.Primary)
                                    .Outlined(),
                                Button("Danger")
                                    .Variant(BootstrapVariant.Danger)
                                    .Outlined()
                            ).HCenter(),
                            
                            Label("Pill Buttons").Muted(),
                            
                            HStack(spacing: 8,
                                Button("Pill Primary")
                                    .Primary()
                                    .Pill(),
                                Button("Pill Success")
                                    .Success()
                                    .Pill()
                            ).HCenter(),
                            
                            Label("Button Sizes").Muted(),
                            
                            HStack(spacing: 8,
                                Button("Large").Primary().Large(),
                                Button("Default").Primary(),
                                Button("Small").Primary().Small()
                            ).HCenter().VCenter()
                        )
                        .Padding(16)
                    )
                    .ShadowSm(),

                    // Entry Section
                    Border(
                        VStack(spacing: 12,
                            Label("Entry Controls").H4(),
                            
                            Entry()
                                .Placeholder("Default Entry"),
                            
                            Entry()
                                .Placeholder("Primary Entry")
                                .Primary(),
                            
                            Entry()
                                .Placeholder("Danger Entry")
                                .Danger(),
                            
                            Entry()
                                .Placeholder("Large Entry")
                                .Large(),
                            
                            Entry()
                                .Placeholder("Small Entry")
                                .Small()
                        )
                        .Padding(16)
                    )
                    .ShadowSm(),

                    // Selection Controls
                    Border(
                        VStack(spacing: 12,
                            Label("Selection Controls").H4(),
                            
                            HStack(spacing: 12,
                                CheckBox().IsChecked(true).Primary(),
                                Label("Primary CheckBox").VCenter()
                            ),
                            
                            HStack(spacing: 12,
                                CheckBox().IsChecked(true).Success(),
                                Label("Success CheckBox").VCenter()
                            ),
                            
                            HStack(spacing: 12,
                                Switch().IsToggled(true).Primary(),
                                Label("Primary Switch").VCenter()
                            ),
                            
                            HStack(spacing: 12,
                                Switch().IsToggled(true).Success(),
                                Label("Success Switch").VCenter()
                            )
                        )
                        .Padding(16)
                    )
                    .ShadowSm(),

                    // Progress Section
                    Border(
                        VStack(spacing: 12,
                            Label("Progress Indicators").H4(),
                            
                            Label("Primary Progress").Muted(),
                            ProgressBar().Progress(0.75).Primary(),
                            
                            Label("Success Progress").Muted(),
                            ProgressBar().Progress(0.5).Success(),
                            
                            Label("Danger Progress").Muted(),
                            ProgressBar().Progress(0.25).Danger(),
                            
                            HStack(spacing: 20,
                                ActivityIndicator().IsRunning(true).Primary(),
                                ActivityIndicator().IsRunning(true).Success(),
                                ActivityIndicator().IsRunning(true).Danger()
                            ).HCenter()
                        )
                        .Padding(16)
                    )
                    .ShadowSm()
                )
                .Padding(20)
            )
        );
}

class TypographyPage : Component
{
    public override VisualNode Render()
        => ContentPage("Typography",
            ScrollView(
                VStack(spacing: 20,
                    // Headings
                    Border(
                        VStack(spacing: 8,
                            Label("Headings").H3(),
                            BoxView().HeightRequest(1).Color(Colors.LightGray),
                            
                            Label("Heading 1 (h1)").H1(),
                            Label("Heading 2 (h2)").H2(),
                            Label("Heading 3 (h3)").H3(),
                            Label("Heading 4 (h4)").H4(),
                            Label("Heading 5 (h5)").H5(),
                            Label("Heading 6 (h6)").H6()
                        )
                        .Padding(20)
                    )
                    .ShadowSm(),

                    // Text Styles
                    Border(
                        VStack(spacing: 12,
                            Label("Text Styles").H3(),
                            BoxView().HeightRequest(1).Color(Colors.LightGray),
                            
                            Label("This is lead text - larger and lighter for introductory content.")
                                .Lead(),
                            
                            Label("This is normal body text. Lorem ipsum dolor sit amet, consectetur adipiscing elit."),
                            
                            Label("This is small text for fine print.")
                                .TextStyle(BootstrapTextStyle.Small),
                            
                            Label("This is muted text for secondary content.")
                                .Muted()
                        )
                        .Padding(20)
                    )
                    .ShadowSm(),

                    // Text Colors
                    Border(
                        VStack(spacing: 8,
                            Label("Text Colors").H3(),
                            BoxView().HeightRequest(1).Color(Colors.LightGray),
                            
                            Label("Primary text color").TextColor(BootstrapVariant.Primary),
                            Label("Secondary text color").TextColor(BootstrapVariant.Secondary),
                            Label("Success text color").TextColor(BootstrapVariant.Success),
                            Label("Danger text color").TextColor(BootstrapVariant.Danger),
                            Label("Warning text color").TextColor(BootstrapVariant.Warning),
                            Label("Info text color").TextColor(BootstrapVariant.Info)
                        )
                        .Padding(20)
                    )
                    .ShadowSm(),

                    // Badges
                    Border(
                        VStack(spacing: 12,
                            Label("Badges").H3(),
                            BoxView().HeightRequest(1).Color(Colors.LightGray),
                            
                            FlexLayout(
                                Label("Primary").Badge(BootstrapVariant.Primary),
                                Label("Secondary").Badge(BootstrapVariant.Secondary),
                                Label("Success").Badge(BootstrapVariant.Success),
                                Label("Danger").Badge(BootstrapVariant.Danger)
                            ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),
                            
                            HStack(spacing: 8,
                                Label("Messages").H5().VCenter(),
                                Label("4").Badge(BootstrapVariant.Primary).VCenter()
                            )
                        )
                        .Padding(20)
                    )
                    .ShadowSm()
                )
                .Padding(20)
            )
        );
}

class CardsPage : Component
{
    public override VisualNode Render()
        => ContentPage("Cards",
            ScrollView(
                VStack(spacing: 20,
                    Label("Cards & Containers").H2().HCenter(),
                    Label("Border control with Bootstrap card styling").Lead().HCenter(),

                    // Shadow Variants
                    Label("Shadow Variants").H4(),
                    
                    Border(
                        VStack(
                            Label("No Shadow").FontAttributes(FontAttributes.Bold),
                            Label("Shadow = None").Muted()
                        ).Padding(16)
                    ),
                    
                    Border(
                        VStack(
                            Label("Small Shadow").FontAttributes(FontAttributes.Bold),
                            Label("ShadowSm()").Muted()
                        ).Padding(16)
                    )
                    .ShadowSm(),
                    
                    Border(
                        VStack(
                            Label("Default Shadow").FontAttributes(FontAttributes.Bold),
                            Label("ShadowMd()").Muted()
                        ).Padding(16)
                    )
                    .ShadowMd(),
                    
                    Border(
                        VStack(
                            Label("Large Shadow").FontAttributes(FontAttributes.Bold),
                            Label("ShadowLg()").Muted()
                        ).Padding(16)
                    )
                    .ShadowLg(),

                    // Border Variants
                    Label("Border Color Variants").H4(),
                    
                    FlexLayout(
                        Border(Label("Primary").HCenter())
                            .Primary().Padding(16).WidthRequest(100),
                        Border(Label("Success").HCenter())
                            .Success().Padding(16).WidthRequest(100),
                        Border(Label("Danger").HCenter())
                            .Danger().Padding(16).WidthRequest(100)
                    ).Wrap(Microsoft.Maui.Layouts.FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                    // Background Variants
                    Label("Background Variants").H4(),
                    
                    Border(
                        VStack(
                            Label("Primary Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                            Label("Background(Primary)").TextColor(Colors.White)
                        ).Padding(16)
                    )
                    .Background(BootstrapVariant.Primary)
                    .ShadowSm(),
                    
                    Border(
                        VStack(
                            Label("Success Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                            Label("Background(Success)").TextColor(Colors.White)
                        ).Padding(16)
                    )
                    .Background(BootstrapVariant.Success)
                    .ShadowSm(),
                    
                    Border(
                        VStack(
                            Label("Dark Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                            Label("Background(Dark)").TextColor(Colors.White)
                        ).Padding(16)
                    )
                    .Background(BootstrapVariant.Dark)
                    .ShadowSm()
                )
                .Padding(20)
            )
        );
}
