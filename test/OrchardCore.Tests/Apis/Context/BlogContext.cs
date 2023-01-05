namespace OrchardCore.Tests.Apis.Context
{
    public class BlogContext : SiteContext
    {
        public const string luceneRecipePath = "Areas/TheBlogTheme/Recipes";
        public const string luceneRecipeName = "blog.lucene.query.recipe.json";
        public const string luceneIndexName = "Search";

        public string BlogContentItemId { get; private set; }

        static BlogContext()
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await RunRecipeAsync(luceneRecipeName, luceneRecipePath);
            await ResetLuceneIndiciesAsync(luceneIndexName);

            var result = await GraphQLClient
                .Content
                .Query("blog", builder =>
                {
                    builder
                        .WithField("contentItemId");
                });

            BlogContentItemId = result["data"]["blog"].First["contentItemId"].ToString();
        }
    }
}
