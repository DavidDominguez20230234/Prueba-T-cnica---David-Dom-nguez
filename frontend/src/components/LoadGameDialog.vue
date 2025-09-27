<script lang="ts" setup>
import { useRouter } from "vue-router";

const props = withDefaults(
  defineProps<{
    modelValue: boolean;
    gameName: string;
    gameExists?: boolean;
  }>(),
  {
    gameExists: false,
  }
);
const emits = defineEmits<{
  (e: "update:modelValue", value: boolean): void;
  (e: "cancel"): void;
  (e: "overwrite"): void;
}>();

const router = useRouter();

const continueGame = () => {
  emits("update:modelValue", false);
  router.push({ name: "game", params: { user: props.gameName } });
};
</script>

<template>
  <!-- Modal resultado -->
  <v-dialog persistent :model-value="modelValue" max-width="400">
    <v-card>
      <div class="d-flex align-center justify-space-between">
        <v-card-title class="text-h6">{{
          gameExists ? "Partida Existente" : "Cargar Partida"
        }}</v-card-title>
        <v-btn
          class="mr-3"
          size="30"
          icon="mdi-close"
          variant="tonal"
          @click="$emit('update:modelValue', false)"
        />
      </div>
      <v-card-text>
        {{
          gameExists
            ? `Ya existe una partida con el nombre: ${gameName}`
            : `¿Desea continuar la partida de ${gameName}?`
        }}
      </v-card-text>
      <v-card-actions>
        <v-btn variant="elevated" color="error" @click="$emit('cancel')"
          >Cancelar</v-btn
        >
        <v-spacer />
        <v-btn variant="elevated" color="info" @click="continueGame"
          >Continuar</v-btn
        >
        <v-btn
          v-if="gameExists"
          variant="elevated"
          color="warning"
          @click="$emit('overwrite')"
          >Sobrescribir</v-btn
        >
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>
