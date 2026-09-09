<script setup>
import { ref } from 'vue'
import WorkOrderList from './components/WorkOrderList.vue'
import WorkOrderForm from './components/WorkOrderForm.vue'
import WorkOrderDetail from './components/WorkOrderDetail.vue'
import MyWorkOrders from './components/MyWorkOrders.vue'

const mode = ref('dispatcher') // 'dispatcher' | 'technician'
const view = ref('list')
const selectedId = ref(null)

function showCreate() { view.value = 'create' }
function showList() { view.value = 'list'; selectedId.value = null }
function showDetail(id) { selectedId.value = id; view.value = 'detail' }
function onCreated(id) { showDetail(id) }
function setMode(m) { mode.value = m; showList() }
</script>

<template>
  <div class="app-shell">
    <header class="app-header">
      <h1>Dispatch</h1>
      <nav class="mode-switch">
        <button :class="{ active: mode === 'dispatcher' }" @click="setMode('dispatcher')">Dispatcher</button>
        <button :class="{ active: mode === 'technician' }" @click="setMode('technician')">My Work Orders</button>
      </nav>
    </header>

    <main class="app-content">
      <div class="card">
        <template v-if="mode === 'dispatcher'">
          <WorkOrderList v-if="view === 'list'" @select="showDetail" @create="showCreate" />
          <WorkOrderForm v-else-if="view === 'create'" @created="onCreated" @cancel="showList" />
        </template>
        <MyWorkOrders v-else-if="mode === 'technician' && view === 'list'" @select="showDetail" />
        <WorkOrderDetail v-if="view === 'detail'" :id="selectedId" @back="showList" />
      </div>
    </main>
  </div>
</template>

<style scoped>
.app-shell {
  min-height: 100vh;
}

.app-header {
  background: var(--color-surface);
  border-bottom: 1px solid var(--color-border);
  padding: 1rem 2rem;
  display: flex;
  align-items: baseline;
  gap: 0.75rem;
}
.app-header h1 {
  margin: 0;
  font-size: 1.4rem;
  color: var(--color-primary);
}
.subtitle {
  color: var(--color-muted);
  font-size: 0.9rem;
}

.app-content {
  max-width: 960px;
  margin: 2rem auto;
  padding: 0 1.5rem;
}

.card {
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
  padding: 1.5rem;
}
</style>