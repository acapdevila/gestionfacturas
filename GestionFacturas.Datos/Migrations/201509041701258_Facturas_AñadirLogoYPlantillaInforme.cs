namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Facturas_AñadirLogoYPlantillaInforme : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Facturas", "NombreArchivoLogo", c => c.String(maxLength: 50));
            AddColumn("dbo.Facturas", "NombreArchivoPlantillaInforme", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Facturas", "NombreArchivoPlantillaInforme");
            DropColumn("dbo.Facturas", "NombreArchivoLogo");
        }
    }
}
