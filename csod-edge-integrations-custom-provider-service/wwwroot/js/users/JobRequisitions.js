var JobRequisitions = Vue.extend({
    template: '<div class="ui container" id="jobrequisition-container">\
            <div class="ui raised blue segment">\
                <div class="ui header">Manage User: {{ User.username }}</div>\
                <div class="ui form">\
                    <div class="field" v-for="item in JobRequisitions">\
                        <label>{{ item.id }}</label>\
                        <label>{{ item.title }}</label>\
                        <label>{{ item.description }}</label>\
                        <label>{{ item.ref }}</label>\
                    </div>\
                </div>\
                <div class="ui form">\
                    <div class="field" v-for="item in JobBoards">\
                        <label>{{ item.title }}</label>\
                        <img v-bind:src="item.iconUrl" height="28" width="100">\
                        <input type="checkbox" v-model="item.selected" />\
                    </div>\
                </div>\
            </div>\
            </div>',
    data: function () {
        return {
            User: {},
            JobRequisitions: {},
            JobBoards: {},
            showPasswordInput: false,
            UserData: JSON.parse(sessionStorage.getItem('userCredentials'))
        }
    },
    created: function () {
        this.fetchData();
    },
    methods: {
        fetchData: function () {
            if (!this.UserData
                || !this.UserData.username
                || !this.UserData.password) {
                router.push({
                    name: 'login'
                });
            }
            var self = this;
            $.ajax({
                contentType: "application/json",
                type: "POST",
                dataType: "json",
                data: JSON.stringify(self.UserData),
                url: "./api/jobrequisitions/",
                statusCode: {
                    400: function () {
                        //we got a bad request, redirect user to login page
                        router.push({
                            name: 'login'
                        });
                    }
                },
                success: function (data) {
                    self.User = data.user;
                    self.JobRequisitions = data.jobrequisitions;
                    self.JobBoards = data.jobboards;
                },
                error: function (data) {
                    router.push({
                        name: 'login'
                    });
                }
            });
        }
    }
});
Vue.component('jobrequisition', JobRequisitions);