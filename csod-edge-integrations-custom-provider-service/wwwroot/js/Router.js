var router = new VueRouter({
    routes: [
        { path: '/', component: ManageUsers },
        { path: '/manage', name: 'manageusers', component: ManageUsers },
        { path: '/manageuser/:id', name: 'manageuser', component: ManageUser }
    ]
});
var app = new Vue({
    router: router
}).$mount('#app');
