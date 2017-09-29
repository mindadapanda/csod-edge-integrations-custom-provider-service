var ManageUser = Vue.extend({
    template: '<div class="ui container" id="manage-user-container">\
            <div class="ui raised blue segment">\
                <div class="ui header">Manage User: {{ User.username }}</div>\
                <div class="ui form">\
                    <h4 class="ui dividing header">User Information</h4>\
                    <div class="two fields">\
                        <div class="disabled field">\
                            <label>Username</label>\
                            <input type="text" placeholder="username" v-model="User.username" />\
                        </div>\
                        <div class="field">\
                            <label>Password - <span style="color: #9f3a38;"> If you change the password, you will be required to re-login.</span></label>\
                            <input v-show="showPasswordInput" type="password" placeholder="password" v-model="User.password" />\
                            <div class="ui small right floated button" v-on:click="changePassword($event)" id="change-password-button">Change Password</div>\
                        </div>\
                    </div>\
                    <h4 class="ui dividing header">User Settings</h4>\
                    <div class="field" v-for="(value, key) in Settings">\
                        <label v-if="keyIsNotId(key)">{{ key }}</label>\
                        <input v-if="keyIsNotId(key)" type="text" v-bind:placeholder="key" v-model="Settings[key]" />\
                    </div>\
                    <div class="ui green fluid button" v-on:click="updateUserSettings()">Update User Settings</div>\
                </div>\
            </div>\
            </div>',
    data: function() {
        return {
            User: {},
            Settings: {},
            showPasswordInput: false,
            UserData: JSON.parse(sessionStorage.getItem('userCredentials'))
        }
    },
    created: function () {
        this.fetchData();
    },
    methods: {
        changePassword: function (event) {
            if (this.showPasswordInput) {
                if (this.User.password == '') {
                    return;
                }
                //if password input is already shown and this button is triggered again we should go and update the password
                var self = this;
                $.ajax({
                    contentType: "application/json",
                    type: "POST",
                    data: JSON.stringify({
                        username: self.UserData.username,
                        password: self.UserData.password,
                        updatedPassword: self.User.password
                    }),
                    url: "./api/user/updatepassword",
                    statusCode: {
                        400: function () {
                            alert('bad request, password not updated');
                        }
                    },
                    success: function (data) {
                        sessionStorage.clear();
                        router.push({
                            name: 'login'
                        });
                    },
                    error: function (data) {
                        alert('bad request, password not updated');
                    }
                });
            }
            else {
                this.User.password = '';
                var button = $(event.currentTarget);
                button.text('Update Password');
                button.addClass('yellow');
                this.showPasswordInput = true;
            }
        },
        updateUserSettings: function () {
            var self = this;
            $.ajax({
                contentType: "application/json",
                type: "POST",
                data: JSON.stringify({
                    username: self.UserData.username,
                    password: self.UserData.password,
                    settings: self.Settings
                }),
                url: "./api/user/updateoraddsettings",
                statusCode: {
                    400: function () {
                        alert('bad request');
                    }
                },
                success: function (data) {
                    location.reload();
                },
                error: function (data) {
                    alert('bad request');
                }
            });
        },
        keyIsNotId: function (key) {
            if (key === "id" || key === "Id" || key === "ID" || key === "userId" || key === "internalUserId") {
                return false;
            }
            return true;
        },
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