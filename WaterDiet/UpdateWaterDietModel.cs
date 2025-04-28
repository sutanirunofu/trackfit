using System.ComponentModel.DataAnnotations;

namespace Fitness.WaterDiet;

public class UpdateWaterDietModel
{
    [Range(1, 100_000, ErrorMessage = "Количество выпитой воды может быть от 1 миллилитра до 100000 миллилитров")]
    public decimal Count { get; set; }
}