var ManageUser = Vue.extend({
    template: '<div class="ui container"> \
            <div class="ui small blue button" v-on:click="backToManageUsers()"><i class="reply icon" />Back to Manage Users</div> \
            <div class="ui raised green segment" > \
                <div class="ui header">Manage {{ User.username }}</div> \
                <div class="ui form"> \
                    <h4 class="ui dividing header">User Information</h4>\
                    <div class="two fields"> \
                        <div class="field"> \
                            <label>Username</label> \
                            <input type="text" placeholder="username" v-model="User.username" /> \
                        </div> \
                        <div class="field"> \
                            <label>Password</label> \
                            <input type="text" placeholder="username" v-model="User.password" /> \
                        </div> \
                    </div > \
                    <h4 class="ui dividing header">User Settings</h4>\
                    <div class="field" v-for="(value, key) in User.settings"> \
                        <label v-if="keyIsNotId(key)">{{ key }}<label> \
                        <input type="text" v-bind:placeholder="value" v-model="User.settings[key]" v-if="keyIsNotId(key)" /> \
                    </div> \
                    <div class="ui green button" v-on:click="updateUser()">Update User</div> \
                    <div class="ui right floated red button" v-on:click="removeUser()">Remove User</div> \
                </div>\
            </div>\
            </div>',
    data: function() {
        return {
            User: {}
        }
    },
    created: function () {
        this.fetchData();
    },
    methods: {
        backToManageUsers: function () {
            router.push({
                name:'manageusers'
            })
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
            var self = this;
            $.ajax({
                contentType: "application/json",
                type: "GET",
                dataType: "json",
                url: "./api/user/"+self.$route.params.id,
                success: function (data) {
                    self.User = data
                },
                error: function (data) {
                    console.log("error");
                }
            });
        }
    }
});
Vue.component('manage-user', ManageUser);