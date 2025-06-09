## Configuração da connection string (exemplo local com SQL Server)

Após clonar o repositório, execute:

bash
dotnet user-secrets init --project RotaViagem.Api/RotaViagem.Api.csproj
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Server=SEU_SERVIDOR;Database=RotasViagemDB;User Id=sa;Password=SENHA_AQUI;Encrypt=True;TrustServerCertificate=True" --project RotaViagem.Api/RotaViagem.Api.csproj
