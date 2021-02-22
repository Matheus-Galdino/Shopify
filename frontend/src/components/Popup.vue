<template>
  <Mask backgroundColor="rgba(0, 0, 0, 0.1)">
    <div class="container">
      <span class="material-icons">clear</span>
      <p>{{ content }}</p>

      <div class="actions">
        <button @click="$emit('cancel')">cancel</button>
        <button class="confirm-btn" @click="$emit('confirm')">Yes</button>
      </div>
    </div>
  </Mask>
</template>

<script>
import Mask from "./Mask.vue";

export default {
  name: "Poupup",
  components: {
    Mask,
  },
  props: {
    content: {
      type: String,
    },
    isConfirmation: {
      type: Boolean,
    },
  },
};
</script>

<style scoped>
.container {
  display: flex;
  flex-direction: column;
  border-radius: 24px;
  background: #ffffff;
  padding: 2rem 3rem 3rem;
}

.material-icons {
  color: #828282;
  align-self: flex-end;
  cursor: pointer;
}

p {
  width: 70%;
  font-weight: 500;
  font-size: 2.3rem;
  line-height: 3rem;
  margin-bottom: 3rem;

  color: #34333a;
}

.actions {
  align-self: flex-end;
}

.actions button {
  border: none;
  color: #34333a;
  font-weight: 500;
  font-size: 1.5rem;
  padding: 2rem 3rem;
  background: transparent;
  cursor: pointer;
}

.actions .confirm-btn {
  color: #fff;
  border-radius: 12px;
  background: #eb5757;
  transition: 0.3s ease-out;
}

.confirm-btn:hover {
  background: #e62d2d;
}
</style>
