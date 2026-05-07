var bobCalypso = Vue.createApp({
    data: function () {
        return {
            myList: null,
            item_one: "",
            item_two: "",
            showModalChart: false,
            showModalAlert: false,
            chart_name: "",
            modal_title: "",
            modal_message: "",
            modal_secondary: "",
            modal_yes_no_action: "",
            modal_image: "",
            modal_image_alt: "",
        };
    },
    mounted: function () {
        if (window.sessionStorage.getItem("bobSkipWelcomeOnce") === "true") {
            window.sessionStorage.removeItem("bobSkipWelcomeOnce");
        } else {
            this.showModalDialog("Welcome", "To Bob Calypso .Net Everywhere", "", "", "img/bob_calypso.png", "The Great and Powerful Bob Calypso");
        }
        this.showMyList();
    },
    methods: {
        showModalDialog: function (title, message, secondary, yesNoAction, image, imageAlt) {
            this.modal_title = title;
            this.modal_message = message;
            this.modal_secondary = secondary;
            this.modal_yes_no_action = yesNoAction;
            this.modal_image = image || "";
            this.modal_image_alt = imageAlt || "";
            this.showModalStatus = false;
            this.showModalAlert = true;
        },
        showMyList: function () {
            axios.get(bobApiUrl('/api/items/my_list'), {
                params: {
                    id: 1
                }
            })
            .then(response => {
                if (response.data && response.data.Success) {
                    this.myList = response.data.MyList;
                } else {
                    this.showModalDialog("Error", "Bob: no data for my catalog", "");
                }
            })
            .catch(error => {
                this.showModalDialog("Unknown Error", "Bob: unknown error retrieving catalog.", error);
            });
        },
        validateForm: function () {
            var error = "";
            if (!this.item_one) {
                error += "Please enter item one<br>";
            }
            if (error) {
                this.showModalDialog("Error", "Bob: please check specification", error);
                return false;
            }

            return true;
        },
        submitItemsForm: function () {
            if (!this.validateForm()) {
                return;
            }
            window.sessionStorage.setItem("bobSkipWelcomeOnce", "true");
            axios.get(bobApiUrl('/api/chart/save_items/'), {
                params: {
                    id: 1,
                    item_one: this.item_one,
                    item_two: this.item_two,
                }
            })
            .then(response => {
                if (response.data && response.data.Success) {
                    this.showMyList();
                } else {
                    this.showModalDialog("Unknown Error", "Bob: error.");
                }
            })
            .catch(error => {
                this.showModalDialog("Unknown Error", "Bob: unknown error.", error);
            });
        },
        showChart: function () {
            this.showModalChart = true;
        },
    }
});

bobCalypso.component('modal_chart', vueChartDefinition);
bobCalypso.component('modal_alert', vueAlertDefinition);
bobCalypso.mount('#bob_calypso');
