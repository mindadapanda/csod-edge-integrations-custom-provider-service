//not used right now, uncomment in Router to use it, not guaranteed to work as some contracts have changed
var ManageUsers = Vue.extend({
    template:
    '<div class="ui container" > \
        <div class="ui large dividing header" id="manage-users-header">Manage Users</div> \
        <div class="ui right floated small green button" id="create-user-button" v-on:click="toggleCreateNewUser()">{{ createNewUserText }}</div> \
        <create-new-user v-show="isCreateUserActive"></create-new-user> \
        <div class="ui container" id="users-container"> \
            <div class="ui segments"> \
                <div class="ui segment" v-for="user in Users"> \
                    <div class="ui large label">user <div class="detail">{{user.username}}</div></div> \
                    <div class="ui right floated blue button" v-on:click="manageUser(user.id)"> Manage User</div> \
                </div> \
            </div> \
        </div> \
    </div >',
    data: function () {
        return {
            Users: [],
            SearchInput: '',
            isCreateUserActive: false,
            createNewUserText: 'Create User'
        }
    },
    created: function () {
        this.fetchData();
    },
    methods: {
        manageUser: function (userId) {
            router.push({
                name: 'manageuser',
                params: {
                    id: userId
                }
            })
        },
        toggleCreateNewUser: function () {
            if (this.isCreateUserActive) {
                this.isCreateUserActive = false;
                this.createNewUserText = 'Create user';
            }
            else {
                this.isCreateUserActive = true;
                this.createNewUserText = 'Close';
            }
        },
        fetchData: function () {
            var self = this;
            $.ajax({
                contentType: "application/json",
                type: "GET",
                dataType: "json",
                url: "./api/users",
                success: function (data) {
                    self.Users = data
                },
                error: function (data) {
                    console.log("error");
                }
            });
        }
    },
    components: {
        'create-new-user': {
            props: ['isActive'],
            template:
            '<div class="ui raised green segment" > \
                <div class="ui header">Create New User</div> \
                <div class="ui form"> \
                    <h4 class="ui dividing header">User Information</h4>\
                    <div class="two fields"> \
                        <div class="field"> \
                            <label>Username</label> \
                            <input type="text" placeholder="username" v-model="UserTemplate.username" /> \
                        </div> \
                        <div class="field"> \
                            <label>Password</label> \
                            <input type="text" placeholder="username" v-model="UserTemplate.password" /> \
                        </div> \
                    </div > \
                    <h4 class="ui dividing header">User Settings</h4>\
                    <div class="field" v-for="(value, key) in UserTemplate.settings"> \
                        <label v-if="keyIsNotId(key)">{{ key }}<label> \
                        <input type="text" v-bind:placeholder="value" v-model="UserTemplate.settings[key]" v-if="keyIsNotId(key)" /> \
                    </div> \
                    <div class="ui green button" v-on:click="addNewUser()"> Create</div> \
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
                        dataType: "json",
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
                keyIsNotId: function (key) {
                    if (key === "id" || key === "Id" || key === "ID") {
                        return false;
                    }
                    return true;
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
Vue.component('manage-users', ManageUsers);