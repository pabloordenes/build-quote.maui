using BuildQuote.App.ViewModels;

namespace BuildQuote.App.Views;

public partial class MaterialPage : ContentPage
{
    private readonly MaterialViewModel _viewModel;

    // Inyectamos el ViewModel
    public MaterialPage(MaterialViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel; // Establecemos el puente de datos
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // Cuando el usuario entra a la pantalla, leemos la base de datos
        await _viewModel.LoadMaterialsAsync();
    }
}