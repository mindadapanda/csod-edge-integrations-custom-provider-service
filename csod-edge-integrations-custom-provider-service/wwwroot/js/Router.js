var router = new VueRouter({
    routes: [
        { path: '/', name: 'login', component: Login },        
        { path: '/manageuser', name: 'manageuser', component: ManageUser },
        { path: '/jobrequisitions', name: 'jobrequisitions', component: JobRequisitions }
    ]
});
var app = new Vue({
    router: router
}).$mount('#app');