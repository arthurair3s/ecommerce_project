using NUnit.Framework;

namespace ecommerce.Tests.Support
{
    public static class AssertHelpers
    {
        public static void AssertTimestampsWereGenerated(DateTime createdAt, DateTime updatedAt, DateTime? deletedAt = null)
        {
            // Valida que não são valores default
            Assert.That(createdAt, Is.Not.EqualTo(default(DateTime)), "CreatedAt não foi gerado");
            Assert.That(updatedAt, Is.Not.EqualTo(default(DateTime)), "UpdatedAt não foi gerado");

            // Valida que são recentes (últimos 5 minutos)
            var cincoMinutosAtras = DateTime.UtcNow.AddMinutes(-5);
            Assert.That(createdAt, Is.GreaterThan(cincoMinutosAtras), "CreatedAt não é recente");
            Assert.That(updatedAt, Is.GreaterThan(cincoMinutosAtras), "UpdatedAt não é recente");

            if (deletedAt.HasValue)
            {
                Assert.That(deletedAt.Value, Is.GreaterThan(cincoMinutosAtras), "DeletedAt não é recente");
            }
            else
            {
                Assert.That(deletedAt, Is.Null, "DeletedAt deveria ser null");
            }
        }

        public static void AssertTimestampsAreValidForList(DateTime createdAt, DateTime updatedAt, DateTime? deletedAt = null)
        {
            // Valida que não são valores default
            Assert.That(createdAt, Is.Not.EqualTo(default(DateTime)), "CreatedAt não foi gerado");
            Assert.That(updatedAt, Is.Not.EqualTo(default(DateTime)), "UpdatedAt não foi gerado");

            // Valida ordem dos timestamps
            Assert.That(updatedAt, Is.GreaterThanOrEqualTo(createdAt), "UpdatedAt não pode ser menor que CreatedAt");

            if (deletedAt.HasValue)
            {
                Assert.That(deletedAt.Value, Is.GreaterThan(createdAt), "DeletedAt deve ser maior que CreatedAt");
            }
            else
            {
                Assert.That(deletedAt, Is.Null, "DeletedAt deveria ser null");
            }
        }
    }
}