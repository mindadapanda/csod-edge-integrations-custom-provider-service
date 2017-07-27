var Login = Vue.extend({
    template: '<div class="ui green raised segment" id="login-container" >\
                    <div class="ui center aligned dividing header">Login</div>\
                    <div class="ui form">\
                        <div class="field">\
                            <label>Username</label>\
                            <input type="text" placeholder="username" v-model="Username" />\
                        </div>\
                        <div class="field">\
                            <label>Password</label>\
                            <input type="text" placeholder="password" v-model="Password" v-on:keyup.enter="login()" />\
                        </div>\
                        <div class="ui fluid green button" v-on:click="login()">Login</div>\
                        <div class="ui basic red fluid label" v-show="showFailedLogin" id="failed-login-label">Failed to Login! Username or Password is incorrect!</div>\
                        <div class="ui horizontal divider">OR</div>\
                        <create-new-user v-show="showCreateNewUser"></create-new-user>\
                        <div class="ui fluid teal button" v-show="!showCreateNewUser" v-on:click="createNewUser()">Create New User</div>\
                    </div>\
                </div>',
    data: function () {
        return {
            Username: '',
            Password: '',
            showCreateNewUser: false,
            showFailedLogin: false
        }
    },
    methods: {
        createNewUser: function () {
            this.showCreateNewUser = !this.showCreateNewUser;
        },
        login: function () {
            var self = this;
            $.ajax({
                contentType: "application/json",
                type: "POST",
                dataType: "json",
                data: JSON.stringify({
                    username: self.Username,
                    password: self.Password
                }),
                url: "./api/user/login",
                statusCode: {
                    400: function () {
                        //we got a bad request, user login doesn't work
                        self.showFailedLogin = true;
                    }
                },
                success: function (data) {
                    //store hashed username and password as part of local session storage
                    var userData = JSON.stringify({
                        username: data.username,
                        password: data.password
                    });
                    sessionStorage.setItem('userCredentials', userData);
                    router.push({
                        name: 'manageuser'
                    });
                },
                error: function (data) {
                    self.showFailedLogin = true;
                }
            });
        }
    },
    components: {
        'create-new-user': {
            template:
            '<div class="ui raised green segment" > \
                <div class="ui header">Create New User</div> \
                <div class="ui form"> \
                    <h4 class="ui dividing header">User Information</h4>\
                        <div class="field"> \
                            <label>Username</label> \
                            <input type="text" placeholder="username" v-model="UserTemplate.username" /> \
                        </div> \
                        <div class="field"> \
                            <label>Password</label> \
                            <input type="text" placeholder="username" v-model="UserTemplate.password" /> \
                        </div> \
                    <div class="ui fluid green button" v-on:click="addNewUser()"> Create</div> \
                </div>\
            </div>',
            data: function () {
                return {
                    UserTemplate: {},
                }
            },
            created: function () {
                this.fetchData();
            },
            methods: {
                addNewUser: function () {
                    var self = this;
                    $.ajax({
                        contentType: "application/json",
                        type: "POST",
                        data: JSON.stringify(
                            self.UserTemplate
                        ),
                        url: "./api/user",
                        success: function (data) {
                            location.reload();
                        },
                        error: function (data) {
                            location.reload();
                        }
                    });
                },
                fetchData: function () {
                    var self = this;
                    $.ajax({
                        contentType: "application/json",
                        type: "GET",
                        dataType: "json",
                        url: "./api/user/gettemplate",
                        success: function (data) {
                            self.UserTemplate = data
                        },
                        error: function (data) {
                            console.log("error");
                        }
                    });
                }
            }
        }
    }
});
Vue.component('login', Login);