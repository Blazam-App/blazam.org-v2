﻿window.productViewed = async () => {
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
    gtag('event', 'conversion', { 'send_to': 'AW-16531657046/fzqrCNrlia4aENai9Mo9' });
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
    gtag('event', 'conversion', { 'send_to': 'AW-16531657046/fzqrCNrlia4aENai9Mo9' });
}
window.googleAdsConversion = async() =>{

  gtag('event', 'conversion', {
      'send_to': 'AW-16531657046/75emCNfb1asZENai9Mo9'
  });
}
window.back = async () => {
    history.back();
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


window.attemptSignIn = async () => {
    //Get the form from the current page
    var form = document.querySelector("form");
    var formData = new FormData();
    //Load the form data
    for (var x = 0; x < form.length; x++) {
        //console.log(form[x].name);
        //console.log(form[x].value);
        formData.append(form[x].name, form[x].value)
    }
    //var data = Array.from(formData);
    //console.log(data);

    var xhr = new XMLHttpRequest();
    var response = await new Promise((resolve, reject) => {
        xhr.onreadystatechange = function () {
            if (this.readyState == 4 && this.status == 200) {
                resolve(xhr.response);
            } else if (this.readyState == 4 && this.status != 200) {
                reject(new Error('Request failed'));
            }
        };
        xhr.open('POST', '/signin');
        xhr.send(formData);
    });
    return response;
};