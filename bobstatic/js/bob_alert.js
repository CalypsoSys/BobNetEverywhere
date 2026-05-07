var vueAlertDefinition = Vue.defineAsyncComponent(function () {
    return axios.get('comp/modal_alert.html')
        .then(response => ({
            template: response.data,
            props: {
                show: Boolean,
                title: String,
                message: String,
                secondary: String,
                confirm: String,
                image: String,
                imageAlt: String
            },
            emits: ['close', 'yes'],
            methods: {
                yesNo: function () {
                    return this.confirm;
                },
                onSubmitDoNothing: function () {
                },
                sayYes: function () {
                    // Some save logic goes here...
                    this.$emit('close');
                    this.$emit('yes', true);
                }
            }
        }));
});
