﻿using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Themes;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class FormsPageState
{
    public string Email { get; set; } = "";
    public string Password { get; set; } = "";
    public bool IsChecked { get; set; }
}

class FormsPage : BasePage<FormsPageState>
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: Bs.Spacing4,
                // Header
                VStack(spacing: Bs.Spacing0,
                    Label("Forms").Class(Bs.H1),
                    Label("Canonical Bootstrap 5.3 form patterns for parity comparison.").Class(Bs.Lead).Class(Bs.TextMuted)
                ),

                // Overview
                VStack(spacing: Bs.Spacing3,
                    Label("Overview").Class(Bs.H4),

                    VStack(spacing: Bs.Spacing2,
                        Label("Email address").Class(Bs.FormLabel).Class(Bs.Small),
                        Entry()
                            .Placeholder("name@example.com")
                            .Keyboard(Keyboard.Email)
                            .Text(State.Email)
                            .OnTextChanged(text => SetState(s => s.Email = text))
                            .Class(Bs.FormControl),
                        Label("We'll never share your email with anyone else.").Class(Bs.FormText)
                    ),

                    VStack(spacing: Bs.Spacing2,
                        Label("Password").Class(Bs.FormLabel).Class(Bs.Small),
                        Entry()
                            .IsPassword(true)
                            .Text(State.Password)
                            .OnTextChanged(text => SetState(s => s.Password = text))
                            .Class(Bs.FormControl)
                    ),

                    HStack(spacing: Bs.Spacing3,
                        CheckBox()
                            .IsChecked(State.IsChecked)
                            .OnCheckedChanged(chk => SetState(s => s.IsChecked = chk))
                            .Class(Bs.FormCheckInput),
                        Label("Remember me").Class(Bs.FormCheckLabel).VCenter()
                    ).Class(Bs.Shadow),

                    Button("Submit").Class(Bs.BtnPrimary).HorizontalOptions(LayoutOptions.Start)
                ),

                // Sizing
                VStack(spacing: Bs.Spacing3,
                    Label("Sizing").Class(Bs.H4),

                    VStack(spacing: Bs.Spacing2,
                        Label("Large input").Class(Bs.FormLabel).Class(Bs.Small),
                        Entry().Placeholder(".form-control-lg").Class(Bs.FormControl).Class(Bs.FormControlLg)
                    ),

                    VStack(spacing: Bs.Spacing2,
                        Label("Default input").Class(Bs.FormLabel).Class(Bs.Small),
                        Entry().Placeholder(".form-control").Class(Bs.FormControl)
                    ),

                    VStack(spacing: Bs.Spacing2,
                        Label("Small select").Class(Bs.FormLabel).Class(Bs.Small),
                        Picker()
                            .Title(".form-select-sm")
                            .ItemsSource(new[] { "Option 1", "Option 2" })
                            .Class(Bs.FormSelect).Class(Bs.FormSelectSm)
                    )
                ),

                // Disabled forms
                VStack(spacing: Bs.Spacing3,
                    Label("Disabled forms").Class(Bs.H4),
                    Label("Disabled fieldset example").Class(Bs.Small).Class(Bs.TextMuted),

                    VStack(spacing: Bs.Spacing2,
                        Label("Disabled input").Class(Bs.FormLabel).Class(Bs.Small),
                        Entry().Placeholder("Disabled input").Class(Bs.FormControl)
                    ),

                    VStack(spacing: Bs.Spacing2,
                        Label("Disabled select menu").Class(Bs.FormLabel).Class(Bs.Small),
                        Picker()
                            .Title("Disabled select")
                            .ItemsSource(new[] { "Disabled select" })
                            .Class(Bs.FormSelect)
                    ),

                    Button("Submit").Class(Bs.BtnPrimary).HorizontalOptions(LayoutOptions.Start)
                ).IsEnabled(false),

                // Form layout
                VStack(spacing: Bs.Spacing3,
                    Label("Form layout").Class(Bs.H4),

                    Grid("Auto,Auto,Auto", "*,*",
                        VStack(spacing: Bs.Spacing2,
                            Label("First name").Class(Bs.FormLabel).Class(Bs.Small),
                            Entry().Class(Bs.FormControl)
                        ).GridRow(0).GridColumn(0),

                        VStack(spacing: Bs.Spacing2,
                            Label("Last name").Class(Bs.FormLabel).Class(Bs.Small),
                            Entry().Class(Bs.FormControl)
                        ).GridRow(0).GridColumn(1),

                        VStack(spacing: Bs.Spacing2,
                            Label("Email").Class(Bs.FormLabel).Class(Bs.Small),
                            Entry().Keyboard(Keyboard.Email).Class(Bs.FormControl)
                        ).GridRow(1).GridColumnSpan(2),

                        VStack(spacing: Bs.Spacing2,
                            Label("Message").Class(Bs.FormLabel).Class(Bs.Small),
                            Editor().HeightRequest(100).Class(Bs.FormControl)
                        ).GridRow(2).GridColumnSpan(2)
                    ).ColumnSpacing(Bs.Spacing3).RowSpacing(Bs.Spacing2)
                )
            ).Padding(20)
        );
}
