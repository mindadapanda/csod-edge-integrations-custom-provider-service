var ManageUser = Vue.extend({
    template: '<div class="ui container" id="manage-user-container"> \
            <div class="ui raised green segment" > \
                <div class="ui header">Manage User: {{ User.username }}</div> \
                <div class="ui form"> \
                    <h4 class="ui dividing header">User Information</h4>\
                    <div class="two fields"> \
                        <div class="field"> \
                            <label>Username</label> \
                            <input type="text" placeholder="username" v-model="User.username" /> \
                        </div> \
                        <div class="field"> \
                            <label>Password</label> \
                            <input type="password" placeholder="username" value="User.password" /> \
                        </div> \
                    </div > \
                    <h4 class="ui dividing header">User Settings</h4>\
                    <div class="ui green button" v-on:click="updateUser()">Update User</div> \
                    <div class="ui right floated red button" v-on:click="removeUser()">Remove User</div> \
                </div>\
            </div>\
            </div>',
    data: function() {
        return {
            User: {},
            Settings: {}
        }
    },
    created: function () {
        this.fetchData();
    },
    methods: {
        getUsername: function () {
            return this.User.username;
        },
        updateUser: function () {
            var self = this;
            $.ajax({
                contentType: "application/json",
                type: "PUT",
                dataType: "json",
                data: JSON.stringify(
                    self.User
                ),
                url: "./api/user/"+self.User.id,
                success: function (data) {
                    location.reload();
                },
                error: function (data) {
                    location.reload();
                }
            });
        },
        removeUser: function () {
            var self = this;
            $.ajax({
                contentType: "application/json",
                type: "DELETE",
                dataType: "json",
                url: "./api/user/"+self.User.id,
                success: function (data) {
                    self.backToManageUsers();
                },
                error: function (data) {
                    self.backToManageUsers();
                }
            });
        },
        keyIsNotId: function (key) {
            if (key === "id" || key === "Id" || key === "ID" || key === "userId") {
                return false;
            }
            return true;
        },
        fetchData: function () {
            var userData = JSON.parse(sessionStorage.getItem('userCredentials'));
            if (!userData
                || !userData.username
                || !userData.password) {
                router.push({
                    name: 'login'
                });
            }
            var self = this;
            $.ajax({
                contentType: "application/json",
                type: "POST",
                dataType: "json",
                data: JSON.stringify(userData),
                url: "./api/getuserandsettings/",
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
                    self.Settings = data.settings;
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
Vue.component('manage-user', ManageUser);