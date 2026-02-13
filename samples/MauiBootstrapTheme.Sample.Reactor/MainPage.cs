using MauiReactor;
using MauiBootstrapTheme.Reactor;
using MauiBootstrapTheme.Theming;

namespace MauiBootstrapTheme.Sample.Reactor;

class MainPage : Component
{
    public override VisualNode Render()
    {
        return new Shell
        {
            new FlyoutItem("Controls")
            {
                new ShellContent()
                    .RenderContent(() => new ControlsPage())
            },
            new FlyoutItem("Typography")
            {
                new ShellContent()
                    .RenderContent(() => new TypographyPage())
            },
            new FlyoutItem("Cards")
            {
                new ShellContent()
                    .RenderContent(() => new CardsPage())
            }
        }
        .Title("Bootstrap Theme")
        .FlyoutBehavior(FlyoutBehavior.Flyout);
    }
}

class ControlsPage : Component
{
    public override VisualNode Render()
    {
        return new ContentPage("Controls")
        {
            new ScrollView
            {
                new VStack(spacing: 20)
                {
                    // Header
                    new Label("Bootstrap Themed Controls")
                        .BsH2()
                        .HCenter(),
                    
                    new Label("MauiReactor fluent API demo")
                        .BsLead()
                        .HCenter(),

                    // Buttons Section
                    new Border
                    {
                        new VStack(spacing: 12)
                        {
                            new Label("Buttons").BsH4(),
                            
                            new HStack(spacing: 8)
                            {
                                new Button("Primary").BsPrimary(),
                                new Button("Secondary").BsSecondary(),
                                new Button("Success").BsSuccess(),
                            }.HCenter(),
                            
                            new HStack(spacing: 8)
                            {
                                new Button("Danger").BsDanger(),
                                new Button("Warning").BsWarning(),
                                new Button("Info").BsInfo(),
                            }.HCenter(),
                            
                            new Label("Outline Buttons").BsMuted(),
                            
                            new HStack(spacing: 8)
                            {
                                new Button("Primary")
                                    .BootstrapVariant(BootstrapVariant.Primary)
                                    .BootstrapOutlined(),
                                new Button("Danger")
                                    .BootstrapVariant(BootstrapVariant.Danger)
                                    .BootstrapOutlined(),
                            }.HCenter(),
                            
                            new Label("Pill Buttons").BsMuted(),
                            
                            new HStack(spacing: 8)
                            {
                                new Button("Pill Primary")
                                    .BsPrimary()
                                    .BootstrapPill(),
                                new Button("Pill Success")
                                    .BsSuccess()
                                    .BootstrapPill(),
                            }.HCenter(),
                            
                            new Label("Button Sizes").BsMuted(),
                            
                            new HStack(spacing: 8)
                            {
                                new Button("Large").BsPrimary().BsLarge(),
                                new Button("Default").BsPrimary(),
                                new Button("Small").BsPrimary().BsSmall(),
                            }.HCenter().VCenter(),
                        }
                        .Padding(16)
                    }
                    .BsShadowSm(),

                    // Entry Section
                    new Border
                    {
                        new VStack(spacing: 12)
                        {
                            new Label("Entry Controls").BsH4(),
                            
                            new Entry()
                                .Placeholder("Default Entry"),
                            
                            new Entry()
                                .Placeholder("Primary Entry")
                                .BsPrimary(),
                            
                            new Entry()
                                .Placeholder("Danger Entry")
                                .BsDanger(),
                            
                            new Entry()
                                .Placeholder("Large Entry")
                                .BsLarge(),
                            
                            new Entry()
                                .Placeholder("Small Entry")
                                .BsSmall(),
                        }
                        .Padding(16)
                    }
                    .BsShadowSm(),

                    // Selection Controls
                    new Border
                    {
                        new VStack(spacing: 12)
                        {
                            new Label("Selection Controls").BsH4(),
                            
                            new HStack(spacing: 12)
                            {
                                new CheckBox().IsChecked(true).BsPrimary(),
                                new Label("Primary CheckBox").VCenter(),
                            },
                            
                            new HStack(spacing: 12)
                            {
                                new CheckBox().IsChecked(true).BsSuccess(),
                                new Label("Success CheckBox").VCenter(),
                            },
                            
                            new HStack(spacing: 12)
                            {
                                new Switch().IsToggled(true).BsPrimary(),
                                new Label("Primary Switch").VCenter(),
                            },
                            
                            new HStack(spacing: 12)
                            {
                                new Switch().IsToggled(true).BsSuccess(),
                                new Label("Success Switch").VCenter(),
                            },
                        }
                        .Padding(16)
                    }
                    .BsShadowSm(),

                    // Progress Section
                    new Border
                    {
                        new VStack(spacing: 12)
                        {
                            new Label("Progress Indicators").BsH4(),
                            
                            new Label("Primary Progress").BsMuted(),
                            new ProgressBar().Progress(0.75).BsPrimary(),
                            
                            new Label("Success Progress").BsMuted(),
                            new ProgressBar().Progress(0.5).BsSuccess(),
                            
                            new Label("Danger Progress").BsMuted(),
                            new ProgressBar().Progress(0.25).BsDanger(),
                            
                            new HStack(spacing: 20)
                            {
                                new ActivityIndicator().IsRunning(true).BsPrimary(),
                                new ActivityIndicator().IsRunning(true).BsSuccess(),
                                new ActivityIndicator().IsRunning(true).BsDanger(),
                            }.HCenter(),
                        }
                        .Padding(16)
                    }
                    .BsShadowSm(),
                }
                .Padding(20)
            }
        };
    }
}

class TypographyPage : Component
{
    public override VisualNode Render()
    {
        return new ContentPage("Typography")
        {
            new ScrollView
            {
                new VStack(spacing: 20)
                {
                    // Headings
                    new Border
                    {
                        new VStack(spacing: 8)
                        {
                            new Label("Headings").BsH3(),
                            new BoxView().HeightRequest(1).Color(Colors.LightGray),
                            
                            new Label("Heading 1 (h1)").BsH1(),
                            new Label("Heading 2 (h2)").BsH2(),
                            new Label("Heading 3 (h3)").BsH3(),
                            new Label("Heading 4 (h4)").BsH4(),
                            new Label("Heading 5 (h5)").BsH5(),
                            new Label("Heading 6 (h6)").BsH6(),
                        }
                        .Padding(20)
                    }
                    .BsShadowSm(),

                    // Text Styles
                    new Border
                    {
                        new VStack(spacing: 12)
                        {
                            new Label("Text Styles").BsH3(),
                            new BoxView().HeightRequest(1).Color(Colors.LightGray),
                            
                            new Label("This is lead text - larger and lighter for introductory content.")
                                .BsLead(),
                            
                            new Label("This is normal body text. Lorem ipsum dolor sit amet, consectetur adipiscing elit."),
                            
                            new Label("This is small text for fine print.")
                                .BootstrapTextStyle(BootstrapTextStyle.Small),
                            
                            new Label("This is muted text for secondary content.")
                                .BsMuted(),
                        }
                        .Padding(20)
                    }
                    .BsShadowSm(),

                    // Text Colors
                    new Border
                    {
                        new VStack(spacing: 8)
                        {
                            new Label("Text Colors").BsH3(),
                            new BoxView().HeightRequest(1).Color(Colors.LightGray),
                            
                            new Label("Primary text color").BootstrapTextColor(BootstrapVariant.Primary),
                            new Label("Secondary text color").BootstrapTextColor(BootstrapVariant.Secondary),
                            new Label("Success text color").BootstrapTextColor(BootstrapVariant.Success),
                            new Label("Danger text color").BootstrapTextColor(BootstrapVariant.Danger),
                            new Label("Warning text color").BootstrapTextColor(BootstrapVariant.Warning),
                            new Label("Info text color").BootstrapTextColor(BootstrapVariant.Info),
                        }
                        .Padding(20)
                    }
                    .BsShadowSm(),

                    // Badges
                    new Border
                    {
                        new VStack(spacing: 12)
                        {
                            new Label("Badges").BsH3(),
                            new BoxView().HeightRequest(1).Color(Colors.LightGray),
                            
                            new FlexLayout
                            {
                                new Label("Primary").BootstrapBadge(BootstrapVariant.Primary),
                                new Label("Secondary").BootstrapBadge(BootstrapVariant.Secondary),
                                new Label("Success").BootstrapBadge(BootstrapVariant.Success),
                                new Label("Danger").BootstrapBadge(BootstrapVariant.Danger),
                            }.Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),
                            
                            new HStack(spacing: 8)
                            {
                                new Label("Messages").BsH5().VCenter(),
                                new Label("4").BootstrapBadge(BootstrapVariant.Primary).VCenter(),
                            },
                        }
                        .Padding(20)
                    }
                    .BsShadowSm(),
                }
                .Padding(20)
            }
        };
    }
}

class CardsPage : Component
{
    public override VisualNode Render()
    {
        return new ContentPage("Cards")
        {
            new ScrollView
            {
                new VStack(spacing: 20)
                {
                    new Label("Cards & Containers").BsH2().HCenter(),
                    new Label("Border control with Bootstrap card styling").BsLead().HCenter(),

                    // Shadow Variants
                    new Label("Shadow Variants").BsH4(),
                    
                    new Border
                    {
                        new VStack
                        {
                            new Label("No Shadow").FontAttributes(FontAttributes.Bold),
                            new Label("BootstrapShadow = None").BsMuted(),
                        }.Padding(16)
                    },
                    
                    new Border
                    {
                        new VStack
                        {
                            new Label("Small Shadow").FontAttributes(FontAttributes.Bold),
                            new Label("BsShadowSm()").BsMuted(),
                        }.Padding(16)
                    }
                    .BsShadowSm(),
                    
                    new Border
                    {
                        new VStack
                        {
                            new Label("Default Shadow").FontAttributes(FontAttributes.Bold),
                            new Label("BsShadow()").BsMuted(),
                        }.Padding(16)
                    }
                    .BsShadow(),
                    
                    new Border
                    {
                        new VStack
                        {
                            new Label("Large Shadow").FontAttributes(FontAttributes.Bold),
                            new Label("BsShadowLg()").BsMuted(),
                        }.Padding(16)
                    }
                    .BsShadowLg(),

                    // Border Variants
                    new Label("Border Color Variants").BsH4(),
                    
                    new FlexLayout
                    {
                        new Border { new Label("Primary").HCenter() }
                            .BsPrimary().Padding(16).WidthRequest(100),
                        new Border { new Label("Success").HCenter() }
                            .BsSuccess().Padding(16).WidthRequest(100),
                        new Border { new Label("Danger").HCenter() }
                            .BsDanger().Padding(16).WidthRequest(100),
                    }.Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start),

                    // Background Variants
                    new Label("Background Variants").BsH4(),
                    
                    new Border
                    {
                        new VStack
                        {
                            new Label("Primary Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                            new Label("BootstrapBackground(Primary)").TextColor(Colors.White),
                        }.Padding(16)
                    }
                    .BootstrapBackground(BootstrapVariant.Primary)
                    .BsShadowSm(),
                    
                    new Border
                    {
                        new VStack
                        {
                            new Label("Success Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                            new Label("BootstrapBackground(Success)").TextColor(Colors.White),
                        }.Padding(16)
                    }
                    .BootstrapBackground(BootstrapVariant.Success)
                    .BsShadowSm(),
                    
                    new Border
                    {
                        new VStack
                        {
                            new Label("Dark Background").FontAttributes(FontAttributes.Bold).TextColor(Colors.White),
                            new Label("BootstrapBackground(Dark)").TextColor(Colors.White),
                        }.Padding(16)
                    }
                    .BootstrapBackground(BootstrapVariant.Dark)
                    .BsShadowSm(),
                }
                .Padding(20)
            }
        };
    }
}
