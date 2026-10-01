using System.ComponentModel.DataAnnotations;

namespace SolicitacoesInternas.Models;

public class Solicitacao
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Solicitante { get; set; } = string.Empty;

    [Required]
    public TipoSolicitacao Tipo { get; set; }

    [Required]
    [MaxLength(500)]
    public string Descricao { get; set; } = string.Empty;

    [Required]
    public StatusSolicitacao Status { get; set; } = StatusSolicitacao.Aberta;

    public DateTime DataSolicitacao { get; set; } = DateTime.Now;
}

public enum TipoSolicitacao
{
    TI,
    Manutencao,
    Material,
    Acesso,
    Outros
}

public enum StatusSolicitacao
{
    Aberta,
    EmAnalise,
    Atendida,
    Cancelada
}
