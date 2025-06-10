function LoadData() {
    const searchTitle = $("#searchTitle").val();
    const userRole = $("#userRole").val();

    $.ajax({
        url: `http://localhost:5153/api/NewsArticles?searchTitle=${searchTitle}`,
        method: 'GET',
        success: (result) => {
            const articles = result.$values || result; // Adjust based on your API response format
            let tr = "";

            $.each(articles, (index, article) => {
                // Extract category name
                const categoryName = article.Category ? article.Category.CategoryName : "N/A";
                
                // Extract tags
                let tagsHtml = "";
                if (article.Tags && article.Tags.$values) {
                    const tags = article.Tags.$values;
                    for (let i = 0; i < tags.length; i++) {
                        tagsHtml += tags[i].TagName;
                        if (i < tags.length - 1) tagsHtml += ", ";
                    }
                }
                
                // Extract created by and updated by
                const createdBy = article.CreatedBy ? article.CreatedBy.FullName : "N/A";
                const updatedBy = article.UpdatedBy ? article.UpdatedBy.FullName : "N/A";

                // Build the table row
                tr += `<tr>
                    <td>${article.NewsTitle || "undefined"}</td>
                    <td>${article.Headline || "undefined"}</td>
                    <td>${categoryName}</td>
                    <td>${tagsHtml}</td>
                    <td>${article.NewsSource || "undefined"}</td>
                    <td>${article.NewsStatus || "false"}</td>
                    <td>${article.CreatedDate || "undefined"}</td>
                    <td>${article.ModifiedDate || "undefined"}</td>
                    <td>${createdBy}</td>
                    <td>${updatedBy}</td>
                    <td>
                        ${userRole === "1"
                        ? `<a href="./NewsArticlePages/Edit?id=${article.NewsArticleId}">Edit</a> | 
                               <a href="./NewsArticlePages/Details?id=${article.NewsArticleId}">Details</a> | 
                               <a href="./NewsArticlePages/Delete?id=${article.NewsArticleId}">Delete</a>`
                        : `<a href="./NewsArticlePages/Details?id=${article.NewsArticleId}">Details</a>`
                        }
                    </td>
                </tr>`;
            });

            // Update the table body
            $("#tableBody").html(tr);
        },
        error: (error) => {
            console.error("Error loading data:", error);
        }
    });
}

$(() => {
    LoadData();

    const connection = new signalR.HubConnectionBuilder()
        .withUrl("/signalrServer")
        .build();

    connection.start()
        .then(() => console.log("SignalR connection established."))
        .catch(err => console.error("SignalR connection failed:", err));

    connection.on("LoadData", function () {
        LoadData();
    });
});
