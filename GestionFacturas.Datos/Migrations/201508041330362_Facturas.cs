namespace GestionFacturas.Datos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Facturas : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Facturas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdUsuario = c.String(nullable: false, maxLength: 128),
                        SerieFactura = c.String(nullable: false, maxLength: 50),
                        NumeracionFactura = c.Int(nullable: false),
                        FormatoNumeroFactura = c.String(nullable: false, maxLength: 50),
                        FechaEmisionFactura = c.DateTime(nullable: false),
                        FechaVencimientoFactura = c.DateTime(nullable: false),
                        IdVendedor = c.Int(),
                        VendedorNumeroIdentificacionFiscal = c.String(maxLength: 50),
                        VendedorNombreOEmpresa = c.String(maxLength: 50),
                        VendedorDireccion = c.String(maxLength: 50),
                        VendedorLocalidad = c.String(maxLength: 50),
                        VendedorProvincia = c.String(maxLength: 10),
                        VendedorCodigoPostal = c.String(maxLength: 50),
                        IdComprador = c.Int(),
                        CompradorNumeroIdentificacionFiscal = c.String(maxLength: 50),
                        CompradorNombreOEmpresa = c.String(maxLength: 50),
                        CompradorDireccion = c.String(maxLength: 50),
                        CompradorLocalidad = c.String(maxLength: 50),
                        CompradorProvincia = c.String(maxLength: 50),
                        CompradorCodigoPostal = c.String(maxLength: 10),
                        EstadoFactura = c.Int(nullable: false),
                        Comentarios = c.String(maxLength: 250),
                        ComentariosPie = c.String(maxLength: 250),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.IdUsuario)
                .Index(t => t.IdUsuario);
            
            CreateTable(
                "dbo.FacturasLineas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IdFactura = c.Int(nullable: false),
                        Descripcion = c.String(maxLength: 250),
                        Cantidad = c.Int(nullable: false),
                        PrecioUnitario = c.Decimal(nullable: false, precision: 18, scale: 2),
                        PorcentajeImpuesto = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Facturas", t => t.IdFactura)
                .Index(t => t.IdFactura);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Facturas", "IdUsuario", "dbo.AspNetUsers");
            DropForeignKey("dbo.FacturasLineas", "IdFactura", "dbo.Facturas");
            DropIndex("dbo.FacturasLineas", new[] { "IdFactura" });
            DropIndex("dbo.Facturas", new[] { "IdUsuario" });
            DropTable("dbo.FacturasLineas");
            DropTable("dbo.Facturas");
        }
    }
}
