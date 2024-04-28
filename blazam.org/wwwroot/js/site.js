window.productViewed = async () => {
    gtag('event', 'view_item', {
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
window.showDonateButton = async () => {
    PayPal.Donation.Button({
        env: 'production',
        hosted_button_id: 'HNDBMY9BX7SJ2',
        image: {
            src: 'https://www.paypalobjects.com/en_US/i/btn/btn_donate_LG.gif',
            alt: 'Donate with PayPal button',
            title: 'PayPal - The safer, easier way to pay online!',
        }
    }).render('#donate-button');
}