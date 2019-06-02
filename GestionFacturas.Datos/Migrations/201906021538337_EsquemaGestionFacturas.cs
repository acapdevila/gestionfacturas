namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EsquemaGestionFacturas : DbMigration
    {
        public override void Up()
        {
            MoveTable(name: "dbo.Clientes", newSchema: "GestionFacturas");
            MoveTable(name: "dbo.Facturas", newSchema: "GestionFacturas");
            MoveTable(name: "dbo.FacturasLineas", newSchema: "GestionFacturas");
        }
        
        public override void Down()
        {
            MoveTable(name: "GestionFacturas.FacturasLineas", newSchema: "dbo");
            MoveTable(name: "GestionFacturas.Facturas", newSchema: "dbo");
            MoveTable(name: "GestionFacturas.Clientes", newSchema: "dbo");
        }
    }
}
