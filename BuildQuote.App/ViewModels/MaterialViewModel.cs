using BuildQuote.Domain.Entities;
using BuildQuote.Domain.Repositories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Xml.Linq;

namespace BuildQuote.App.ViewModels;

// DEBE ser "partial" para que el Toolkit genere el código oculto.
public partial class MaterialViewModel : ObservableObject
{
    private readonly IMaterialRepository _repository;

    // ObservableCollection notifica a la UI automáticamente cuando se agrega/quita un elemento.
    [ObservableProperty]
    private ObservableCollection<Material> _materials = new();

    // Propiedades del formulario
    [ObservableProperty]
    private string _name = string.Empty;

    [ObservableProperty]
    private decimal _unitCost;

    [ObservableProperty]
    private UnitOfMeasure _selectedUnit = UnitOfMeasure.Unidad;

    // Exponemos el Enum para que el Picker de XAML pueda mostrar las opciones
    public UnitOfMeasure[] Units { get; } = (UnitOfMeasure[])Enum.GetValues(typeof(UnitOfMeasure));

    public MaterialViewModel(IMaterialRepository repository)
    {
        _repository = repository;
    }

    // [RelayCommand] convierte este método en un ICommand que podemos llamar desde botones en XAML
    [RelayCommand]
    public async Task LoadMaterialsAsync()
    {
        var data = await _repository.GetAllActiveAsync();
        Materials.Clear();
        foreach (var item in data)
        {
            Materials.Add(item);
        }
    }

    [RelayCommand]
    public async Task AddMaterialAsync()
    {
        if (string.IsNullOrWhiteSpace(Name) || UnitCost <= 0)
            return; // Validación simple

        var newMaterial = new Material
        {
            Name = this.Name,
            UnitCost = this.UnitCost,
            Unit = this.SelectedUnit
        };

        // 1. Guardamos en SQLite
        await _repository.AddAsync(newMaterial);

        // 2. Actualizamos la lista visual instantáneamente
        Materials.Add(newMaterial);

        // 3. Limpiamos el formulario
        Name = string.Empty;
        UnitCost = 0;
        SelectedUnit = UnitOfMeasure.Unidad;
    }

    [RelayCommand]
    public async Task DeleteMaterialAsync(Material material)
    {
        if (material == null) return;

        // 1. Borrado lógico en SQLite (Soft Delete)
        await _repository.SoftDeleteAsync(material.Id);

        // 2. Lo quitamos de la pantalla
        Materials.Remove(material);
    }
}