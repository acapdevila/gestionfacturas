namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migracion_Factura_NombreComprador128 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("GestionFacturas.Facturas", "CompradorNombreOEmpresa", c => c.String(maxLength: 128));
        }
        
        public override void Down()
        {
            AlterColumn("GestionFacturas.Facturas", "CompradorNombreOEmpresa", c => c.String(maxLength: 50));
        }
    }
}
