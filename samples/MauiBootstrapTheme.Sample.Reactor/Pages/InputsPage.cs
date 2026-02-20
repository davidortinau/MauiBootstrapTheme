﻿﻿using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Themes;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class InputsPage : BasePage
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: 24,
                // Page Header
                VStack(spacing: 4,
                    Label("Input Controls").Class(Bs.H1),
                    Label("Form inputs with Bootstrap styling").Class(Bs.Lead).Class(Bs.TextMuted)
                ),

                // Text Inputs
                VStack(spacing: 12,
                    Label("Text Inputs").Class(Bs.H4),
                    VStack(spacing: 4,
                        Label("Default Input").Class(Bs.FormLabel),
                        Entry().Placeholder("Enter text...").Class(Bs.FormControl)
                    ),
                    VStack(spacing: 4,
                        Label("Large Input").Class(Bs.FormLabel),
                        Entry().Placeholder("Large input").Class(Bs.FormControl).Class(Bs.FormControlLg)
                    ),
                    VStack(spacing: 4,
                        Label("Small Input").Class(Bs.FormLabel),
                        Entry().Placeholder("Small input").Class(Bs.FormControl).Class(Bs.FormControlSm)
                    )
                ),

                // Textarea
                VStack(spacing: 12,
                    Label("Textarea").Class(Bs.H4),
                    VStack(spacing: 4,
                        Label("Default Textarea").Class(Bs.FormLabel),
                        Editor().Placeholder("Enter multi-line text...").HeightRequest(100).Class(Bs.FormControl)
                    )
                ),

                // Select
                VStack(spacing: 12,
                    Label("Select").Class(Bs.H4),
                    VStack(spacing: 4,
                        Label("Default Select").Class(Bs.FormLabel),
                        Picker().Title("Choose an option...").ItemsSource(new[] { "Option 1", "Option 2", "Option 3" }).Class(Bs.FormSelect)
                    )
                ),

                // Date & Time
                VStack(spacing: 12,
                    Label("Date & Time").Class(Bs.H4),
                    Grid("Auto", "*,*",
                        VStack(spacing: 4,
                            Label("Date").Class(Bs.FormLabel),
                            DatePicker().Class(Bs.FormControl)
                        ).GridColumn(0),
                        VStack(spacing: 4,
                            Label("Time").Class(Bs.FormLabel),
                            TimePicker().Class(Bs.FormControl)
                        ).GridColumn(1)
                    ).ColumnSpacing(20)
                ),

                // Checkboxes & Switches
                VStack(spacing: 12,
                    Label("Checkboxes & Switches").Class(Bs.H4),
                    HStack(spacing: 8,
                        CheckBox().IsChecked(true).Class(Bs.FormCheckInput).Color(GetResource<Color>("Primary")),
                        Label("Default checkbox").Class(Bs.FormCheckLabel).VCenter()
                    ),
                    HStack(spacing: 8,
                        CheckBox().Class(Bs.FormCheckInput).Color(GetResource<Color>("Primary")),
                        Label("Unchecked checkbox").Class(Bs.FormCheckLabel).VCenter()
                    ),
                    HStack(spacing: 8,
                        Switch().IsToggled(true).Class(Bs.FormSwitch).OnColor(GetResource<Color>("Primary")),
                        Label("Enabled switch").Class(Bs.FormCheckLabel).VCenter()
                    ),
                    HStack(spacing: 8,
                        Switch().Class(Bs.FormSwitch).OnColor(GetResource<Color>("Primary")),
                        Label("Disabled switch").Class(Bs.FormCheckLabel).VCenter()
                    )
                ),

                // Radio Buttons
                VStack(spacing: 12,
                    Label("Radio Buttons").Class(Bs.H4),
                    VStack(
                        RadioButton().Content("Option 1").IsChecked(true).Class(Bs.FormCheckInput),
                        RadioButton().Content("Option 2").Class(Bs.FormCheckInput),
                        RadioButton().Content("Option 3").Class(Bs.FormCheckInput)
                    ).Set(Microsoft.Maui.Controls.RadioButtonGroup.GroupNameProperty, "radioGroup")
                ),

                // Range Slider
                VStack(spacing: 12,
                    Label("Range Slider").Class(Bs.H4),
                    VStack(spacing: 4,
                        Label("Default Range").Class(Bs.FormLabel),
                        Slider().Minimum(0).Maximum(100).Value(50).Class(Bs.FormRange)
                            .MinimumTrackColor(GetResource<Color>("Primary"))
                            .ThumbColor(GetResource<Color>("Primary"))
                    )
                )
            ).Padding(20)
        );
}
