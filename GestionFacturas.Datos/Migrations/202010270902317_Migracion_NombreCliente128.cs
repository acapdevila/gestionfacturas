namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracion_NombreCliente128 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("GestionFacturas.Clientes", "NombreOEmpresa", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("GestionFacturas.Clientes", "NombreOEmpresa", c => c.String(maxLength: 50));
        }
    }
}
