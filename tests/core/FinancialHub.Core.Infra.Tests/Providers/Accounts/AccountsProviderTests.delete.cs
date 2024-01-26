namespace FinancialHub.Core.Infra.Tests.Providers
{
    public partial class AccountsProviderTests
    {
        [Test]
        public async Task DeleteAsync_ReturnsAmountOfLinesDeleted()
        {
            var deletedLines = 1;
            var guid = Guid.NewGuid();

            repository
                .Setup(x => x.DeleteAsync(guid))
                .ReturnsAsync(deletedLines);
            repository
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(deletedLines);

            var result = await this.provider.DeleteAsync(guid);

            Assert.That(result, Is.EqualTo(deletedLines));
        }
    }
}
