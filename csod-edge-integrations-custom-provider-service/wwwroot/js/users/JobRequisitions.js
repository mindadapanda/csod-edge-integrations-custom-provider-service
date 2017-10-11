var JobRequisitions = Vue.extend({
    template: '<div class="ui container" id="jobrequisition-container">\
            <div class="ui active inverted dimmer" v-show="loading"> \
                <div class="ui text loader">Loading</div> \
            </div> \
                <h1> Nelson\'s Job Poster 3000!</h1> \
                <div class="ui container" id="nelson-face-container"> \
                    <img src="/images/nelson.png" height="80" id="nelson-face"> \
                </div> \
            <h2 class="ui dividing header">Welcome, {{ User.username }}</h2> \
            <h3 class="ui header">Job Requisitions</h3> \
            <div class="ui very relaxed list"> \
                <div class="item" v-for="req in JobRequisitions"> \
                    <div class="ui right floated content"> \
                        <div class="ui green icon button"> \
                            <i class="edit icon"></i> Post \
                        </div> \
                    </div> \
                    <div class="content"> \
                        <a class="large header">{{ req.title}} &nbsp;&nbsp;<a class="ui mini blue label">{{ req.ref }}</a></a> \
                        <div class="description"> {{ req.description }}</div> \
                    </div> \
                    <br /> \
                    <h4 class="ui dividing header">Select Job Boards: </h4> \
                    <br /> \
                    <div class="ui horizontal divided list"> \
                        <div class="item" v-for="jb in JobBoards"> \
                            <img class="ui image" v-bind:src="jb.iconUrl" width="70"> \
                            <div class="content"> \
                                <div class="inline field"> \
                                    <div class="ui toggle checkbox"> \
                                        <input type="checkbox" tabindex="0" class="hidden"> \
                                        <label></label> \
                                    </div> \
                                </div> \
                            </div> \
                        </div> \
                    </div> \
                </div> \
            </div> \
            </div>',
    data: function () {
        return {
            User: {},
            JobRequisitions: {},
            JobBoards: {},
            showPasswordInput: false,
            UserData: JSON.parse(sessionStorage.getItem('userCredentials')),
            loading: false
        }
    },
    created: function () {
        var userdata = JSON.stringify({
            Username: 'utah',
            Password: 'bof5qjpy9rsf5bkr21bindrtrn28qtnpgzfvbmjx8b4='
        });
        sessionStorage.setItem('usercredentials', userdata);
        this.userdata = JSON.parse(userdata);

        this.fetchData();
    },
    watch: {
        loading: function (val) {
            // done loading
            if (!val) {
                setTimeout(function () {
                    $('.ui.checkbox').checkbox();
                }, 1500)
                
            }
        }
    },
    methods: {
        fetchData: function () {
            this.loading = true;
            if (!this.UserData
                || !this.UserData.username
                || !this.UserData.password) {
                this.loading = false;
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
                        self.loading = false;
                    }
                },
                success: function (data) {
                    self.User = data.user;
                    self.JobRequisitions = data.jobrequisitions;
                    self.JobBoards = data.jobboards;
                    self.loading = false;
                },
                error: function (data) {
                    router.push({
                        name: 'login'
                    });
                    self.loading = false;
                }
            });
        }
    }
});
Vue.component('jobrequisition', JobRequisitions);