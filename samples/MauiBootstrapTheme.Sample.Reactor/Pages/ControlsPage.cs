﻿using MauiReactor;
using MauiBootstrapTheme.Extensions;
using MauiBootstrapTheme.Theming;
using MauiBootstrapTheme.Sample.Reactor.Themes;

namespace MauiBootstrapTheme.Sample.Reactor.Pages;

class ControlsPage : BasePage
{
    public override VisualNode RenderContent()
        => ScrollView(
            VStack(spacing: Bs.Spacing3,
                // Page Header
                VStack(spacing: Bs.Spacing1,
                    Label("Controls").Class(Bs.H1),
                    Label("Stock .NET MAUI controls styled with Bootstrap 5 theme values.").Class(Bs.TextWarning).Class(Bs.TextMuted)
                ),

                BoxView().HeightRequest(1).Color(BootstrapTheme.Current.OutlineVariant).Margin(0, 4),

                // Buttons Section
                VStack(spacing: Bs.Spacing2,
                    Label("Buttons").Class(Bs.H4),
                    FlexLayout(
                        Button("Primary").Class(Bs.BtnPrimary).Margin(0, 0, 8, 8),
                        Button("Secondary").Class(Bs.BtnSecondary).Margin(0, 0, 8, 8),
                        Button("Success").Class(Bs.BtnSuccess).Margin(0, 0, 8, 8),
                        Button("Danger").Class(Bs.BtnDanger).Margin(0, 0, 8, 8),
                        Button("Warning").Class(Bs.BtnWarning).Margin(0, 0, 8, 8),
                        Button("Info").Class(Bs.BtnInfo).Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Outline Buttons Section
                VStack(spacing: Bs.Spacing2,
                    Label("Outline Buttons").Class(Bs.H5),
                    FlexLayout(
                        Button("Primary").Class(Bs.BtnOutlinePrimary).Margin(0, 0, 8, 8),
                        Button("Secondary").Class(Bs.BtnOutlineSecondary).Margin(0, 0, 8, 8),
                        Button("Success").Class(Bs.BtnOutlineSuccess).Margin(0, 0, 8, 8),
                        Button("Danger").Class(Bs.BtnOutlineDanger).Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Button Sizes Section
                VStack(spacing: Bs.Spacing2,
                    Label("Button Sizes").Class(Bs.H5),
                    FlexLayout(
                        Button("Large").Class(Bs.BtnPrimary).Class(Bs.BtnLg).Margin(0, 0, 8, 8),
                        Button("Default").Class(Bs.BtnPrimary).Margin(0, 0, 8, 8),
                        Button("Small").Class(Bs.BtnPrimary).Class(Bs.BtnSm).Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Pill Buttons Section
                VStack(spacing: Bs.Spacing2,
                    Label("Pill Buttons").Class(Bs.H5),
                    FlexLayout(
                        Button("Primary Pill").Class(Bs.BtnPrimary).Class(Bs.BtnPill).Margin(0, 0, 8, 8),
                        Button("Success Pill").Class(Bs.BtnSuccess).Class(Bs.BtnPill).Margin(0, 0, 8, 8)
                    ).Wrap(FlexWrap.Wrap).JustifyContent(Microsoft.Maui.Layouts.FlexJustify.Start).AlignItems(Microsoft.Maui.Layouts.FlexAlignItems.Center)
                ),

                // Progress Section
                VStack(spacing: Bs.Spacing3,
                    Label("Progress").Class(Bs.H4),
                    VStack(spacing: Bs.Spacing1,
                        Label("Primary (75%)"),
                        ProgressBar().Progress(0.75)
                    ),
                    VStack(spacing: Bs.Spacing1,
                        Label("Success (50%)"),
                        ProgressBar().Progress(0.5).Class(Bs.ProgressSuccess)
                    ),
                    VStack(spacing: Bs.Spacing1,
                        Label("Danger (25%)"),
                        ProgressBar().Progress(0.25).Class(Bs.ProgressDanger)
                    )
                ),

                // Spinners Section
                VStack(spacing: Bs.Spacing2,
                    Label("Spinners").Class(Bs.H4),
                    HStack(spacing: Bs.Spacing3,
                        ActivityIndicator().IsRunning(true).Color(BootstrapTheme.Current.Muted),
                        ActivityIndicator().IsRunning(true).Color(BootstrapTheme.Current.Success),
                        ActivityIndicator().IsRunning(true).Color(BootstrapTheme.Current.Danger)
                    )
                )
            ).Padding(20)
        );
}
