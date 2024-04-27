window.productViewed = async () => {
    gtag('event', 'view_product', {
        'event_category': 'engagement',  // You can customize this category
        'event_label': 'download_page'        // Replace with your product identifier
    });
}


window.webInstallerDownloaded = async () => {
    gtag('event', 'add_to_cart', {
        'currency': 'USD',  // Replace with your currency code
        'value': 0,  // Replace with the product price variable
        'items': [
            {
                'item_id': 'bwi',  // Replace with your product identifier
                'item_name': 'Blazam Web Installer',  // Replace with the product name
                'item_category': 'Blazam',  // Replace with the product category (optional)
                'quantity': 1  // Quantity added (defaults to 1)
            }
        ]
    });
}
window.zipDownloaded = async () => {
    gtag('event', 'add_to_cart', {
        'currency': 'USD',  // Replace with your currency code
        'value': 0,  // Replace with the product price variable
        'items': [
            {
                'item_id': 'brz',  // Replace with your product identifier
                'item_name': 'Blazam Release Zip',  // Replace with the product name
                'item_category': 'Blazam',  // Replace with the product category (optional)
                'quantity': 1  // Quantity added (defaults to 1)
            }
        ]
    });
}