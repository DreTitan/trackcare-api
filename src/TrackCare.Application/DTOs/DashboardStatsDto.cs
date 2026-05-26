namespace TrackCare.Application.DTOs;

public record DashboardStatsDto(
    int TotalAtivos,
    int AguardaColeta,
    int EmReparo,
    int EncerradosMes
);
