﻿using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Themes;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class InputsPage : BasePage
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: Bs.Spacing4,
                // Page Header
                VStack(spacing: Bs.Spacing1,
                    Label("Input Controls").Class(Bs.H1),
                    Label("Form inputs with Bootstrap styling").Class(Bs.Lead).Class(Bs.TextMuted)
                ),

                BoxView().HeightRequest(1).Color(BootstrapTheme.Current.Outline),

                // Text Inputs
                VStack(spacing: Bs.Spacing3,
                    Label("Text Inputs").Class(Bs.H4),
                    VStack(spacing: Bs.Spacing1,
                        Label("Default Input").Class(Bs.FormLabel),
                        Entry().Placeholder("Enter text...").Class(Bs.FormControl)
                    ),
                    VStack(spacing: Bs.Spacing1,
                        Label("Large Input").Class(Bs.FormLabel),
                        Entry().Placeholder("Large input").Class(Bs.FormControl).Class(Bs.FormControlLg)
                    ),
                    VStack(spacing: Bs.Spacing1,
                        Label("Small Input").Class(Bs.FormLabel),
                        Entry().Placeholder("Small input").Class(Bs.FormControl).Class(Bs.FormControlSm)
                    )
                ),

                // Textarea
                VStack(spacing: Bs.Spacing3,
                    Label("Textarea").Class(Bs.H4),
                    VStack(spacing: Bs.Spacing1,
                        Label("Default Textarea").Class(Bs.FormLabel),
                        Editor().Placeholder("Enter multi-line text...").HeightRequest(100).Class(Bs.FormControl)
                    )
                ),

                // Select
                VStack(spacing: Bs.Spacing3,
                    Label("Select").Class(Bs.H4),
                    VStack(spacing: Bs.Spacing1,
                        Label("Default Select").Class(Bs.FormLabel),
                        Picker().Title("Choose an option...").ItemsSource(new[] { "Option 1", "Option 2", "Option 3" }).Class(Bs.FormSelect)
                    )
                ),

                // Date & Time
                VStack(spacing: Bs.Spacing3,
                    Label("Date & Time").Class(Bs.H4),
                    Grid("Auto", "*,*",
                        VStack(spacing: Bs.Spacing1,
                            Label("Date").Class(Bs.FormLabel),
                            DatePicker().Class(Bs.FormControl)
                        ).GridColumn(0),
                        VStack(spacing: Bs.Spacing1,
                            Label("Time").Class(Bs.FormLabel),
                            TimePicker().Class(Bs.FormControl)
                        ).GridColumn(1)
                    ).ColumnSpacing(Bs.Spacing4)
                ),

                // Checkboxes & Switches
                VStack(spacing: Bs.Spacing3,
                    Label("Checkboxes & Switches").Class(Bs.H4),
                    HStack(spacing: Bs.Spacing2,
                        CheckBox().IsChecked(true).Class(Bs.FormCheckInput).Color(GetResource<Color>("Primary")),
                        Label("Default checkbox").Class(Bs.FormCheckLabel).VCenter()
                    ),
                    HStack(spacing: Bs.Spacing2,
                        CheckBox().Class(Bs.FormCheckInput).Color(GetResource<Color>("Primary")),
                        Label("Unchecked checkbox").Class(Bs.FormCheckLabel).VCenter()
                    ),
                    HStack(spacing: Bs.Spacing2,
                        Switch().IsToggled(true).Class(Bs.FormSwitch).OnColor(GetResource<Color>("Primary")),
                        Label("Enabled switch").Class(Bs.FormCheckLabel).VCenter()
                    ),
                    HStack(spacing: Bs.Spacing2,
                        Switch().Class(Bs.FormSwitch).OnColor(GetResource<Color>("Primary")),
                        Label("Disabled switch").Class(Bs.FormCheckLabel).VCenter()
                    )
                ),

                // Radio Buttons
                VStack(spacing: Bs.Spacing3,
                    Label("Radio Buttons").Class(Bs.H4),
                    VStack(
                        RadioButton().Content("Option 1").IsChecked(true).Class(Bs.FormCheckInput),
                        RadioButton().Content("Option 2").Class(Bs.FormCheckInput),
                        RadioButton().Content("Option 3").Class(Bs.FormCheckInput)
                    ).Set(Microsoft.Maui.Controls.RadioButtonGroup.GroupNameProperty, "radioGroup")
                ),

                // Range Slider
                VStack(spacing: Bs.Spacing3,
                    Label("Range Slider").Class(Bs.H4),
                    VStack(spacing: Bs.Spacing1,
                        Label("Default Range").Class(Bs.FormLabel),
                        Slider().Minimum(0).Maximum(100).Value(50).Class(Bs.FormRange)
                            .MinimumTrackColor(GetResource<Color>("Primary"))
                            .ThumbColor(GetResource<Color>("Primary"))
                    )
                )
            ).Padding(20)
        );
}
