var router = new VueRouter({
    routes: [
        { path: '/', name: 'login', component: Login },
        //{ path: '/manage', name: 'manageusers', component: ManageUsers },
        { path: '/manageuser', name: 'manageuser', component: ManageUser }
    ]
});
var app = new Vue({
    router: router
}).$mount('#app');