import GameView from "@/views/GameView.vue";
import StartView from "@/views/StartView.vue";
import { createRouter, createWebHistory } from "vue-router";

const routes = [
  { path: "/", name: "start", component: StartView },
  { path: "/game/:user", name: "game", component: GameView },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
