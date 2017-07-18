define(["sitecore"], function (Sitecore) {
    var InsertLinkPC = Sitecore.Definitions.App.extend({
        initialized: function () {
            var app = this;
            app.Target.set("CheckedItems", app.Target.viewModel.$el.attr("data-sc-selecteditem"));
            app.TargetsDataSource.on("change:hasItems", function () {
                var a = app.TargetsDataSource["attributes"].items;
                a.forEach(function (item, i, a) {
                    if (item.Text == app.Target["attributes"].CheckedItems) {
                        app.Target.set("selectedItem", item);
                    }
                });
            })
        }
    });

    return InsertLinkPC;
});